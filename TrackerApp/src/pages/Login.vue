<template>
  <q-layout view="lHh Lpr lFf">
    <div class="full-width row justify-center items-center fixed-center">
      <div
        class="transition justify-center row wra q-col-pa-sm q-col-gutter-sm col-xs-10 col-sm-8 col-md-6 col-lg-3 col-xl-3"
      >
        <!-- Logo
        <div class="text-center col-xs-8 col-sm-8 col-md-6 col-lg-10 col-xl-6">
          <img src="../../assets/Pic_Login.png" width="100%" />
        </div>-->

        <!-- VivaCare -->
        <div
          class="text-center col-xs-12 col-sm-12 col-md-12 col-lg-10 col-xl-12"
        >
          <span class="color-primary text-h2 text-weight-small">Package</span>
          <span class="color-primary text-h4">Tracker</span>
        </div>

        <!-- Espaciador -->
        <div class="col-12">
          <br />
        </div>

        <!-- Usuario -->
        <div class="col-xs-12 col-sm-10 col-md-10 col-lg-12 col-xl-8">
          <q-input v-model="Usuario" dense outlined rounded label="Correo">
            <template v-slot:before>
              <q-icon name="mdi-at"></q-icon>
            </template>
          </q-input>
          <br />
        </div>
        <!-- Password -->
        <div class="col-xs-12 col-sm-10 col-md-10 col-lg-12 col-xl-8">
          <q-input
            ref="InputPassword"
            v-model="Password"
            dense
            outlined
            rounded
            :type="ShowPass ? 'password' : 'text'"
            label="Password"
            @keypress.enter="Login(Usuario, Password)"
          >
            <template v-slot:before>
              <q-icon name="mdi-form-textbox-password"></q-icon>
            </template>
            <template v-slot:append>
              <q-icon
                :name="ShowPass ? 'mdi-eye' : 'mdi-eye-off'"
                @click="ShowPass = !ShowPass"
              ></q-icon>
            </template>
          </q-input>
        </div>
        <!-- ¿Olvidaste tu contraseña? -->
        <!-- <div
          class="text-right col-xs-12 col-sm-10 col-md-10 col-lg-12 col-xl-8"
        >
          <q-chip
            text-color="black"
            color="white"
            size="sm"
            clickable
            @click="SolicitarPassword = true"
            >Olvidaste tu contraseña</q-chip
          >
        </div> -->

        <!-- Espaciador -->
        <div class="col-xs-12">
          <br />
          <br />
        </div>

        <!-- Boton Iniciar Sesión -->
        <div class="text-center col-xs-8 col-sm-8 col-md-6 col-lg-12 col-xl-6">
          <q-btn
            color="primary"
            class="btn-primary full-width"
            label="Iniciar Sesión"
            @click="Login(Usuario, Password)"
            no-caps
            flat
          ></q-btn>
        </div>
      </div>

      <!-- Espaciador -->
      <div class="col-xs-12">
        <br />
        <br />
      </div>
    </div>
  </q-layout>
</template>

<script>
import { request } from "@isatol/fetchmodule";
export default {
  data() {
    return {
      Usuario: "",
      Password: "",
      ShowPass: true
    };
  },
  beforeCreate() {
    if (localStorage.getItem("token")) {
      location.replace("home");
    }
  },
  methods: {
    Login(user, password) {
      request("Auth/Login", {
        method: "post",
        data: JSON.stringify({
          Email: user,
          Password: password
        })
      }).then(res => {
        if (res.code === 1) {
          localStorage.setItem("token", res.token);
          location.replace("home");
        } else {
          this.$q.notify({
            type: "negative",
            message: "Usuario o contraseña no válidos",
            color: "red"
          });
        }
      });
    }
  }
};
</script>

<style></style>
