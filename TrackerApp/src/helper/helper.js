import { date } from "quasar";

const months = [
  "Enero",
  "Febrero",
  "Marzo",
  "Abril",
  "Mayo",
  "Junio",
  "Julio",
  "Agosto",
  "Septiembre",
  "Octubre",
  "Noviembre",
  "Diciembre"
];

const formatDate = (datetoFormat, format) => {
  return date.formatDate(datetoFormat, format, {
    months: months
  });
};

export { months, formatDate };
