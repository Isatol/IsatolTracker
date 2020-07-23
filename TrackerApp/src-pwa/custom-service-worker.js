/*
 * This file (which will be your service worker)
 * is picked up by the build system ONLY if
 * quasar.conf > pwa > workboxPluginMode is set to "InjectManifest"
 */

import { precacheAndRoute } from "workbox-precaching";

self.addEventListener("push", evt => {
  const objNotification = evt.data.json();
  const options = {
    body: objNotification.notification.body,
    icon: objNotification.notification.icon,
    vibrate: [200, 100, 200, 100]
  };
  self.registration.showNotification(
    objNotification.notification.title,
    options
  );
});

precacheAndRoute(self.__WB_MANIFEST);
