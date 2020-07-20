// import something here

// "async" is optional;
// more info on params: https://quasar.dev/quasar-cli/cli-documentation/boot-files#Anatomy-of-a-boot-file
import { createInstance, useWithHeaders } from "@isatol/fetchmodule";
export default async (/* { app, router, Vue ... } */) => {
  // something to do
  console.log(process.env.API + "Auth/Login");
  createInstance("http://localhost:5000/api/");
  const headers = new Headers();
  headers.append("Content-Type", "application/json");
  if (sessionStorage.getItem("token")) {
    headers.append(
      "Authorization",
      `Bearer ${sessionStorage.getItem("token")}`
    );
  }
  useWithHeaders(headers);
};
