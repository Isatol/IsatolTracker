<template>
  <q-page class="flex flex-center">
    <!-- <img
      alt="Quasar logo"
      src="~assets/quasar-logo-full.svg"
    > -->
    <q-btn
      label="Permitir notificaciones"
      @click="PermitirNotificaciones()"
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
      inputTrackingNumber: ""
    };
  },
  methods: {
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
