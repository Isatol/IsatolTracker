const rules = {
  correo: v =>
    /^\w+([.-]?\w+)*@\w+([.-]?\w+)*(\.\w{2,6})+$/.test(v) ||
    "Correo electrónico no válido",
  requerido: v => !!v || "Este valor es requerido"
};

export { rules };
