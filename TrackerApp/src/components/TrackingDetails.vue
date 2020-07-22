<template>
  <q-card>
    <q-card-section class="bg-primary text-white">
      <q-item>
        <q-item-label>{{ packageName }}</q-item-label>
      </q-item>
      <q-list>
        <q-item v-if="trackingModel.delivered">
          <q-item-section avatar>
            <q-icon name="mdi-truck-check" color="white"></q-icon>
          </q-item-section>
          <q-item-section>
            <q-item-label>Entregado</q-item-label>
            <q-item-label class="text-caption"
              >Tu paquete fue entregado</q-item-label
            >
          </q-item-section>
        </q-item>
        <q-item>
          <q-item-section avatar>
            <q-icon name="mdi-truck" color="white"></q-icon>
          </q-item-section>
          <q-item-section>
            <q-item-label>Estado actual</q-item-label>
            <q-item-label class="text-caption">{{
              trackingModel.status
            }}</q-item-label>
          </q-item-section>
        </q-item>
        <q-item>
          <q-item-section avatar>
            <q-icon name="mdi-calendar-today" color="white"></q-icon>
          </q-item-section>
          <q-item-section>
            <q-item-label>Fecha estimada de llegada</q-item-label>
            <q-item-label class="text-caption">{{
              trackingModel.estimateDelivery == null
                ? "Sin fecha estimada"
                : formatDate(trackingModel.estimateDelivery, "DD-MM-YYYY HH:mm")
            }}</q-item-label>
          </q-item-section>
        </q-item>
      </q-list>
    </q-card-section>
    <q-card-section>
      <q-table
        :grid="$q.screen.xs"
        class="my-sticky-header-table"
        :data="trackingModel.trackingDetails"
        :columns="columns"
      >
      </q-table>
    </q-card-section>
  </q-card>
</template>

<script>
import { formatDate } from "../helper/helper";
export default {
  name: "TrackingDetails",
  props: {
    trackingModel: {
      type: Object,
      required: true
    },
    packageName: {
      type: String,
      required: true
    }
  },
  data() {
    return {
      formatDate,
      columns: [
        {
          name: "date",
          field: row => formatDate(row.date, "DD-MM-YYYY HH:mm"),
          label: "Fecha"
        },
        {
          name: "event",
          field: "event",
          label: "Evento"
        },
        {
          name: "messages",
          field: "messages",
          label: "Adicionales"
        }
      ]
    };
  }
};
</script>
