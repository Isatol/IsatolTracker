// import something here

import { route } from "quasar/wrappers";

// "async" is optional;
// more info on params: https://quasar.dev/quasar-cli/cli-documentation/boot-files#Anatomy-of-a-boot-file
const jwtDecode = require("jwt-decode");
export default async ({ router }) => {
  // something to do
  router.beforeEach((to, from, next) => {
    const token = localStorage.getItem("token");
    let isExpired;
    if (token !== null) {
      const decodedData = jwtDecode(token);
      const now = new Date();
      const expToken = new Date(1000 * decodedData.exp);
      // console.log(now);
      // console.log(expToken);
      if (now > expToken) {
        isExpired = true;
        localStorage.removeItem("token");
        next({
          name: "login"
        });
      }
    }
    if (to.path === "/" && token != null) {
      next({
        name: "home"
      });
    } else if (to.name === "home" && token == null) {
      // next({
      //   name: "login"
      // });
      router.replace("/");
      // location.replace("/");
    }
    next();
  });
};
