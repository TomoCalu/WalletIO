<template>
  <div class="alert alert-warning alert-dismissible" role="alert">
    {{alert.message}}
  </div>
</template>

<script>
import { mapState, mapActions } from "vuex";

export default {
  name: "alert",
  props: ["message"],
  data() {
    return {};
  },
  computed: {
    ...mapState({
      alert: state => state.alert
    })
  },
  methods: {
    ...mapActions({
      clearAlert: "alert/clear"
    })
  },
  watch: {
    $route(to, from) {
      // clear alert on location change
      this.clearAlert();
    }
  }
};
</script>