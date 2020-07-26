// import something here

// "async" is optional;
// more info on params: https://quasar.dev/quasar-cli/cli-documentation/boot-files#Anatomy-of-a-boot-file
import VueGtag, { VueGtagPlugin } from "vue-gtag";
export default async ({ app, router, Vue }) => {
  // something to do
  Vue.use(
    VueGtag,
    {
      config: { id: "UA-173518964-1" }
    },
    router
  );
};
