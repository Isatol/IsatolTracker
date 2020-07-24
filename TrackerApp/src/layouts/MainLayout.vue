<template>
  <q-layout view="lHh Lpr lFf">
    <q-header elevated>
      <q-toolbar>
        <q-toolbar-title>
          Package Tracker
        </q-toolbar-title>

        <div>
          <q-btn icon="mdi-account" :label="`Hola ${user.name}`">
            <q-menu>
              <q-list style="mint-width: 100px">
                <div v-if="isNotificationPermited">
                  <q-item
                    clickable
                    v-close-popup
                    v-if="!thereIsASubscription"
                    @click="PermitirNotificaciones()"
                  >
                    <q-item-section avatar>
                      <q-icon name="mdi-bell-ring"></q-icon>
                    </q-item-section>
                    <q-item-section>Permitir notificaciones</q-item-section>
                  </q-item>
                  <q-item
                    clickable
                    v-close-popup
                    v-else
                    @click="EliminarSuscripcion()"
                  >
                    <q-item-section avatar>
                      <q-icon name="mdi-bell-off"></q-icon>
                    </q-item-section>
                    <q-item-section>Desactivar notificaciones</q-item-section>
                  </q-item>
                </div>
                <q-item
                  clickable
                  v-close-popup
                  v-if="!isUserReceiveEmails"
                  @click="UpdateReceiveEmails(true)"
                >
                  <q-item-section avatar>
                    <q-icon name="mdi-email"></q-icon>
                  </q-item-section>
                  <q-item-section>Recibir alertas por correo</q-item-section>
                </q-item>
                <q-item
                  clickable
                  v-close-popup
                  v-else
                  @click="UpdateReceiveEmails(false)"
                >
                  <q-item-section avatar>
                    <q-icon name="mdi-email-off"></q-icon>
                  </q-item-section>
                  <q-item-section>Desactivar alertas por correo</q-item-section>
                </q-item>
                <q-item clickable v-close-popup @click="Logout()">
                  <q-item-section avatar>
                    <q-icon name="mdi-logout"></q-icon>
                  </q-item-section>
                  <q-item-section>Cerrar sesión</q-item-section>
                </q-item>
              </q-list>
            </q-menu>
          </q-btn>
        </div>
      </q-toolbar>
    </q-header>

    <q-page-container>
      <router-view />
    </q-page-container>
  </q-layout>
</template>

<script>
import { request } from "@isatol/fetchmodule";
const jwtDecode = require("jwt-decode");
export default {
  name: "MainLayout",
  data() {
    return {
      leftDrawerOpen: false,
      essentialLinks: [],
      isUserReceiveEmails: false
    };
  },
  created() {
    this.SaveUser();
  },
  // beforeCreate() {
  //   if (!localStorage.getItem("token")) {
  //     this.$router.replace("/");
  //   } else {
  //     const jwt = jwtDecode(localStorage.getItem("token"));
  //     const now = new Date();
  //     const expToken = new Date(1000 * jwt.exp);
  //     if (now > expToken) {
  //       this.$router.replace("/");
  //     }
  //   }
  // },
  mounted() {
    this.IsUserReceiveEmails();
  },
  computed: {
    isNotificationPermited() {
      if (window.Notification) return true;
      return false;
    },
    user() {
      return this.$store.getters["user/getUser"];
    },
    thereIsASubscription() {
      console.log(this.$store.getters["user/getSubscriptionState"]);
      return this.$store.getters["user/getSubscriptionState"];
    }
  },
  methods: {
    UpdateReceiveEmails(receiveEmails) {
      request("Auth/UpdateReceiveEmails", {
        method: "post",
        data: JSON.stringify({
          UsersID: this.user.userID,
          ReceiveEmails: receiveEmails
        })
      }).then(response => {
        this.$q.notify({
          type: "positive",
          message: response.message,
          progress: true
        });
        this.IsUserReceiveEmails();
      });
    },
    IsUserReceiveEmails() {
      request("Auth/IsUserReceiveEmails", {
        method: "get",
        params: {
          userID: this.user.userID
        }
      }).then(response => {
        if (response.code === 1) {
          this.isUserReceiveEmails = response.receiveEmails;
          console.log(this.isUserReceiveEmails);
        }
      });
    },
    SaveUser() {
      const userLocalStorage = localStorage.getItem("token");
      const decodedUser = jwtDecode(userLocalStorage);
      const user = {};
      user.userID = parseInt(decodedUser.userID);
      user.name = decodedUser.name;
      user.email = decodedUser.email;
      this.$store.commit("user/saveUser", user);
    },
    PermitirNotificaciones() {
      Notification.requestPermission().then(permiso => {
        if (permiso === "granted") {
          request("Notification/GetVapidPublicKey", {
            method: "get"
          }).then(res => {
            navigator.serviceWorker.ready.then(register => {
              register.pushManager
                .subscribe({
                  userVisibleOnly: true,
                  applicationServerKey: res.publicKey
                })
                .then(resSubscription => {
                  register.pushManager.getSubscription().then(subscription => {
                    request("Notification/AddNotificationSubscription", {
                      method: "post",
                      data: JSON.stringify({
                        PushSubscription: subscription.toJSON(),
                        UserID: 1
                      })
                    }).then(() => {
                      register.showNotification("Notificaciones activas", {
                        body: "Estás recibiendo notificaciones "
                      });
                      this.$store.commit("user/changeSubscriptionState", true);
                    });
                  });
                });
            });
          });
        }
      });
    },
    EliminarSuscripcion() {
      navigator.serviceWorker.ready.then(sw => {
        sw.pushManager.getSubscription().then(subscription => {
          const jsonSubscription = subscription.toJSON();
          subscription.unsubscribe().then(() => {
            request("Notification/DeleteSubscription", {
              method: "post",
              data: JSON.stringify({
                UsersID: this.$store.getters["user/getUser"].userID,
                Endpoint: jsonSubscription.endpoint
              })
            })
              .then(response => {
                console.log(response);
                this.$q.notify({
                  message: response.message,
                  type: "positive",
                  progress: true
                });
              })
              .finally(
                this.$store.commit("user/changeSubscriptionState", false)
              );
          });
        });
      });
    },
    Logout() {
      localStorage.removeItem("token");
      this.$router.replace("/");
    }
  }
};
</script>
