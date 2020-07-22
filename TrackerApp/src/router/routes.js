import TrackingDetails from "pages/TrackingDetails.vue";
const routes = [
  {
    path: "/",
    // component: () => import("layouts/MainLayout.vue")
    component: () => import("pages/Login.vue")
    // children: [{ path: "", component: () => import("pages/Index.vue") }]
  },
  {
    path: "/home",
    component: () => import("layouts/MainLayout.vue"),
    children: [
      {
        path: "",
        component: () => import("pages/Index.vue"),
        children: [
          {
            path: "track/:companyID",
            name: "trackingPackage",
            components: {
              tracking: TrackingDetails
            }
          }
        ]
      }
    ]
  },
  // Always leave this as last one,
  // but you can also remove it
  {
    path: "*",
    component: () => import("pages/Error404.vue")
  }
];

export default routes;
