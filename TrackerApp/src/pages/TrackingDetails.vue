<template>
  <q-page padding>
    <div class="row justify-center">
      <q-table
        :columns="columns"
        :title="trackingModel.delivered ? 'Entregado' : null"
        :data="trackingModel.trackingDetails"
      >
        <template slot="body" slot-scope="props">
          <q-tr :props="props">
            <q-td key="date" :props="props">
              {{ FormatDate(props.row.date) }}
            </q-td>
            <q-td key="event" :props="props">
              {{ props.row.event }}
            </q-td>
            <q-td key="messages" :props="props">
              {{ props.row.messages }}
            </q-td>
          </q-tr>
        </template>
      </q-table>
    </div>
  </q-page>
</template>

<script>
import { formatDate } from "../helper/helper";
import { request } from "@isatol/fetchmodule";
export default {
  data() {
    return {
      trackingModel: {},
      columns: [
        {
          name: "date",
          label: "Fecha"
        },
        {
          name: "event",
          label: "Evento"
        },
        {
          name: "messages",
          label: "Información"
        }
      ]
    };
  },
  created() {
    this.Track();
  },
  watch: {
    trackingNumber(val) {
      this.Track();
    }
  },
  computed: {
    trackingNumber() {
      return this.$route.query.trackingNumber;
    }
  },
  methods: {
    FormatDate(date) {
      console.log(date);
      return formatDate(date, "DD-MM-YYYY HH:mm");
    },
    Track() {
      const number = this.$route.query.trackingNumber;
      const company = this.$route.params.companyID;
      request("Track/GetTrackingModel", {
        method: "get",
        params: {
          companyID: company,
          trackingNumber: number
        }
      }).then(res => {
        this.trackingModel = res.trackingModel;
        console.log(this.trackingModel);
      });
    }
  }
};
</script>

<style scoped>
.table {
  width: 200px;
  width: 100%;
}
</style>
