<template>
  <q-page class="items-center justify-center row">
    <!-- <img
      alt="Quasar logo"
      src="~assets/quasar-logo-full.svg"
    > -->
    <!-- <div class="col-12">
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
    </div> -->
    <div class="q-pa-lg">
      <q-table
        grid
        :filter="filter"
        card-class="bg-primary text-white"
        title="Mis paquetes"
        :data="packagesList"
        :columns="columns"
      >
        <template v-slot:top-right>
          <q-input
            borderless
            dense
            debounce="300"
            v-model="filter"
            placeholder="Buscar"
          >
            <template v-slot:append>
              <q-icon name="search" />
            </template>
          </q-input>
        </template>
        <template slot="item" slot-scope="props">
          <div
            :class="
              filter.length > 0
                ? 'q-pa-xs q-pl-xs col-xs-12 col-sm-6 col-md-12'
                : 'q-pa-xs q-pl-xs col-xs-12 col-sm-6 col-md-3'
            "
          >
            <q-card bordered>
              <div class="row ">
                <div class="col">
                  <q-item>
                    <q-item-section avatar>
                      <q-avatar size="28px">
                        <img :src="props.row.logo" />
                      </q-avatar>
                    </q-item-section>
                    <q-item-section>
                      <q-item-label class="text-subtitle1">{{
                        props.row.packageName
                      }}</q-item-label>
                      <q-item-label class="text-caption"
                        >{{ props.row.name }}
                        {{ props.row.trackingNumber }}</q-item-label
                      >
                    </q-item-section>
                  </q-item>
                </div>
                <div class="col-auto">
                  <q-btn color="red" size="md" round flat icon="mdi-delete">
                  </q-btn>
                </div>
              </div>
              <q-separator></q-separator>
              <q-card-section class="q-pt-xs">
                <q-list>
                  <q-item>
                    <q-item-section avatar>
                      <q-icon color="primary" name="mdi-calendar"></q-icon>
                    </q-item-section>
                    <q-item-section>
                      <q-item-label>{{
                        props.row.date == "0001-01-01T00:00:00"
                          ? "En espera de actualización"
                          : FormatDate(props.row.date)
                      }}</q-item-label>
                    </q-item-section>
                  </q-item>
                  <q-item>
                    <q-item-section avatar>
                      <q-icon color="accent" name="mdi-truck-fast"></q-icon>
                    </q-item-section>
                    <q-item-section>
                      <q-item-label>{{
                        props.row.event === null
                          ? "En espera de actualización"
                          : props.row.event
                      }}</q-item-label>
                    </q-item-section>
                  </q-item>
                </q-list>
              </q-card-section>
              <q-separator></q-separator>
              <q-card-actions align="right">
                <q-btn
                  flat
                  color="primary"
                  label="Ver más"
                  @click="
                    GetTrackingModel(
                      props.row.companyID,
                      props.row.trackingNumber,
                      props.row.packageName
                    )
                  "
                ></q-btn>
              </q-card-actions>
            </q-card>
          </div>
        </template>
      </q-table>
    </div>
    <q-dialog persistent v-model="dialogTrackingDetails">
      <q-card style="max-width: 800px">
        <q-card-section class="row" v-if="loadingDetails">
          <div class="row">
            <q-spinner color="primary" size="lg"></q-spinner>
          </div>
        </q-card-section>
        <q-card-section v-else>
          <TrackingDetails
            :trackingModel="trackingModel"
            :packageName="packageName"
          ></TrackingDetails>
        </q-card-section>
        <q-card-actions align="right" v-if="!loadingDetails">
          <q-btn
            label="Salir"
            flat
            color="primary"
            @click="
              trackingModel = {};
              dialogTrackingDetails = false;
            "
          ></q-btn>
        </q-card-actions>
      </q-card>
    </q-dialog>
    <q-dialog persistent v-model="dialogNewPackage">
      <q-card style="width: 300px">
        <q-card-section>
          <div class="text-h6">Agrega un paquete</div>
        </q-card-section>
        <q-card-section>
          <q-input v-model="inputTrackingNumber" label="Número de rastreo">
            <template v-slot:prepend> <q-icon name="send" /> </template
          ></q-input>
          <q-input
            v-model="packageName"
            hint="Opcional"
            label="Nombre del paquete"
          >
            <template v-slot:prepend>
              <q-icon name="mdi-package-variant-closed"></q-icon>
            </template>
          </q-input>
          <q-separator dark />
          <q-select
            class="q-mt-md"
            standout="bg-primary text-white"
            v-model="company"
            :options="companiesList"
            option-value="companyID"
            option-label="name"
            label="Seleccionar paquetería"
          >
            <template slot="prepend">
              <q-icon name="mdi-truck"></q-icon>
            </template>
            <template slot="option" slot-scope="props">
              <q-item v-bind="props.itemProps" v-on="props.itemEvents">
                <q-item-section avatar>
                  <q-avatar size="28px">
                    <img :src="props.opt.logo" />
                  </q-avatar>
                </q-item-section>
                <q-item-section>
                  <q-item-label v-html="props.opt.name"></q-item-label>
                </q-item-section>
              </q-item>
            </template>
          </q-select>
        </q-card-section>
        <q-card-actions align="right">
          <q-btn flat label="Cancelar" @click="CleanInputTracker()"></q-btn>
          <q-btn
            flat
            label="Agregar"
            :disable="isDisabled"
            :loading="loadingNewPackage"
            @click="AddPackage()"
          >
            <template slot="loading">
              <q-spinner-tail></q-spinner-tail>
            </template>
          </q-btn>
        </q-card-actions>
      </q-card>
    </q-dialog>
    <q-page-sticky position="bottom-right" :offset="[18, 18]">
      <q-btn @click="dialogNewPackage = true" fab icon="add" color="primary" />
    </q-page-sticky>
  </q-page>
</template>

<script>
import TrackingDetails from "../components/TrackingDetails";
import { request } from "@isatol/fetchmodule";
import { formatDate } from "../helper/helper";
export default {
  components: {
    TrackingDetails
  },
  name: "PageIndex",
  data() {
    return {
      loadingNewPackage: false,
      loadingDetails: false,
      packageName: "",
      trackingModel: {},
      dialogTrackingDetails: false,
      filter: "",
      columns: [
        {
          name: "name",
          field: row => row.name
        },
        {
          name: "packageName",
          field: "packageName"
        }
      ],
      dialogNewPackage: false,
      inputTrackingNumber: "",
      companiesList: [],
      company: null,
      packagesList: [],
      thumbStyle: {
        right: "4px",
        borderRadius: "5px",
        backgroundColor: "#027be3",
        width: "5px",
        opacity: 0.75
      },
      barStyle: {
        right: "2px",
        borderRadius: "9px",
        backgroundColor: "#027be3",
        width: "9px",
        opacity: 0.2
      }
    };
  },
  watch: {
    company(val) {
      console.log(val);
    }
  },
  created() {
    this.ThereIsASubscription();
    this.GetCompanies();
  },
  mounted() {
    this.GetUserPackages();
  },
  computed: {
    isDisabled() {
      return (
        this.inputTrackingNumber === null ||
        this.inputTrackingNumber === undefined ||
        this.inputTrackingNumber === "" ||
        this.company === null
      );
    },
    thereIsASubscription() {
      console.log(this.$store.getters["user/getSubscriptionState"]);
      return this.$store.getters["user/getSubscriptionState"];
    }
  },
  methods: {
    GetTrackingModel(companyID, trackingNumber, packageName) {
      if (this.loadingDetails) return;
      this.loadingDetails = true;
      this.dialogTrackingDetails = true;
      request("Track/GetTrackingModel", {
        method: "get",
        params: {
          companyID: companyID,
          trackingNumber: trackingNumber
        }
      })
        .then(response => {
          this.packageName = packageName;
          this.trackingModel = response.trackingModel;
          console.log(this.trackingModel);
        })
        .finally(() => {
          this.loadingDetails = false;
        });
    },
    FormatDate(date) {
      return formatDate(date, "DD-MMMM-YYYY HH:mm");
    },
    GetUserPackages() {
      request("Track/GetUserPackages", {
        method: "get",
        params: {
          userID: this.$store.getters["user/getUser"].userID
        }
      }).then(res => {
        this.packagesList = res.packages;
        console.log(this.packagesList);
      });
    },
    AddPackage() {
      if (this.loadingNewPackage) return;
      this.loadingNewPackage = true;
      request("Track/InsertPackage", {
        method: "post",
        data: JSON.stringify({
          CompanyID: this.company.companyID,
          TrackingNumber: this.inputTrackingNumber,
          UsersID: this.$store.getters["user/getUser"].userID,
          Name: this.packageName
        })
      })
        .then(res => {
          console.log(res);
        })
        .finally(() => {
          this.dialogNewPackage = false;
          this.CleanInputTracker();
          this.loadingNewPackage = false;
          this.GetUserPackages();
        });
    },
    CleanInputTracker() {
      this.inputTrackingNumber = "";
      this.company = null;
      this.packageName = "";
      this.dialogNewPackage = false;
    },
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
<style>
.my-menu-link {
  color: white;
  background: #1976d2;
}
</style>
