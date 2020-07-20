<template>
  <q-page class="flex flex-center">
    <!-- <img
      alt="Quasar logo"
      src="~assets/quasar-logo-full.svg"
    > -->
    <q-btn
      v-if="!thereIsASubscription"
      label="Permitir notificaciones"
      @click="PermitirNotificaciones()"
    ></q-btn>
    <q-btn
      v-else
      label="Eliminar suscripción"
      @click="EliminarSuscripcion()"
    ></q-btn>
    <q-dialog v-model="dialogNewPackage">
      <q-card style="width: 300px">
        <q-card-section>
          <div class="text-h6">Agrega un paquete</div>
        </q-card-section>
        <q-separator dark />
        <q-card-section>
          <q-input v-model="inputTrackingNumber" label="Número de rastreo">
            <template v-slot:prepend> <q-icon name="send" /> </template
          ></q-input>
          <q-select
            standout="bg-teal text-white"
            v-model="company"
            :options="companiesList"
            option-value="companyID"
            option-label="name"
            label="Seleccionar paquetería"
          />
        </q-card-section>
      </q-card>
    </q-dialog>
    <q-page-sticky position="bottom-right" :offset="[18, 18]">
      <q-btn @click="dialogNewPackage = true" fab icon="add" color="primary" />
    </q-page-sticky>
  </q-page>
</template>

<script>
import { request } from "@isatol/fetchmodule";
export default {
  name: "PageIndex",
  data() {
    return {
      dialogNewPackage: false,
      inputTrackingNumber: "",
      companiesList: [],
      company: ""
    };
  },
  created() {
    this.ThereIsASubscription();
    this.GetCompanies();
  },
  computed: {
    thereIsASubscription() {
      console.log(this.$store.getters["user/getSubscriptionState"]);
      return this.$store.getters["user/getSubscriptionState"];
    }
  },
  methods: {
    GetCompanies() {
      request("Companies/GetCompanies", {
        method: "get"
      }).then(response => {
        this.companiesList = response.companies;
        console.log(this.companiesList);
      });
    },
    ThereIsASubscription() {
      navigator.serviceWorker.ready.then(reg => {
        reg.pushManager.getSubscription().then(subscription => {
          if (!subscription) {
            this.$store.commit("user/changeSubscriptionState", false);
          } else {
            this.$store.commit("user/changeSubscriptionState", true);
          }
        });
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
              })
              .finally(
                this.$store.commit("user/changeSubscriptionState", false)
              );
          });
        });
      });
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
    }
  }
};
</script>
