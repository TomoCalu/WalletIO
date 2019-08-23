<template>
  <div class="bg-gradient-primary" style="height: 100vh;">
    <!-- Page Wrapper -->
    <div id="wrapper" v-bind:class="{'sidebar-toggled': isSidebarToggled}">
      <!-- Sidebar -->
      <ul class="navbar-nav bg-gradient-primary sidebar sidebar-dark accordion" v-if="account.status.loggedIn == true" 
        v-bind:class="{toggled: isSidebarToggled}" id="accordionSidebar">
        <!-- Sidebar - Brand -->
        <router-link class="sidebar-brand d-flex align-items-center justify-content-center" to="/dashboard">
          <div class="sidebar-brand-icon rotate-n-15">
            <i class="fas fa-wallet"></i>
          </div>
          <div class="sidebar-brand-text mx-3">WalletIO</div>
        </router-link>

        <!-- Divider -->
        <hr class="sidebar-divider my-0" />

        <!-- Nav Item - Dashboard -->
        <li class="nav-item">
          <router-link class="nav-link" to="/dashboard">
            <i class="fas fa-fw fa-tachometer-alt"></i>
            <span>Dashboard</span>
          </router-link>
        </li>

        <!-- Nav Item - Tables -->
        <li class="nav-item">
          <router-link class="nav-link" to="/records">
            <i class="fas fa-fw fa-table"></i>
            <span>Records</span>
          </router-link>
        </li>

        <!-- Nav Item - Analytics -->
        <li class="nav-item">
          <router-link class="nav-link" to="/analytics">
            <i class="fas fa-fw fa-chart-area"></i>
            <span>Analytics</span>
          </router-link>
        </li>

        <!-- Divider -->
        <hr class="sidebar-divider d-none d-md-block" />

        <!-- Sidebar Toggler (Sidebar) -->
        <div class="text-center d-none d-md-inline">
          <button class="rounded-circle border-0" id="sidebarToggle" v-on:click="sidebarToggler()"></button>
        </div>
      </ul>
      <!-- End of Sidebar -->

      <!-- Content Wrapper -->
      <div id="content-wrapper" class="d-flex flex-column" v-if="account.status.loggedIn == true">
        <!-- Main Content -->
        <div id="content">
          <!-- Topbar -->
          <nav class="navbar navbar-expand navbar-light bg-white topbar mb-4 static-top shadow">
            <!-- Sidebar Toggle (Topbar) -->
            <button id="sidebarToggleTop" class="btn btn-link d-md-none rounded-circle mr-3" v-on:click="isSidebarToggled = !isSidebarToggled">
              <i class="fa fa-bars"></i>
            </button>

            <!-- Topbar Navbar -->
            <ul class="navbar-nav ml-auto">
              <!-- Nav Item - User Information -->
              <li class="nav-item dropdown no-arrow">
                <a class="nav-link dropdown-toggle" href="#" id="userDropdown" role="button" data-toggle="dropdown"
                  aria-haspopup="true" aria-expanded="false">
                  <span class="mr-2 d-none d-lg-inline text-gray-600 small">
                    {{account.user.firstName}} {{account.user.lastName}}
                  </span>
                  <img class="img-profile rounded-circle" src="https://uemwi2gfx64176wmkmj29ejj-wpengine.netdna-ssl.com/wp-content/uploads/2018/01/blank-person-male-300x200.png"/>
                </a>
                <!-- Dropdown - User Information -->
                <div class="dropdown-menu dropdown-menu-right shadow animated--grow-in" aria-labelledby="userDropdown">
                  <a class="dropdown-item" href="#" data-toggle="modal" data-target="#logoutModal">
                    <i class="fas fa-sign-out-alt fa-sm fa-fw mr-2 text-gray-400"></i>
                    Logout
                  </a>
                </div>
              </li>
            </ul>
          </nav>
          <!-- End of Topbar -->

          <div>
            <Alert v-if="alert.message" :class="`alert ${alert.type}`" />
            <router-view></router-view>
          </div>
        </div>
        <!-- End of Main Content -->

        <!-- Footer -->
        <footer class="sticky-footer bg-white">
          <div class="container my-auto">
            <div class="copyright text-center my-auto">
              <span>Copyright &copy; Tomislav Ćalušić 2019</span>
            </div>
          </div>
        </footer>
        <!-- End of Footer -->
      </div>
      <!-- End of Content Wrapper -->
    </div>
    <!-- End of Page Wrapper -->

    <!-- Scroll to Top Button-->
    <a class="scroll-to-top rounded" href="#page-top">
      <i class="fas fa-angle-up"></i>
    </a>

    <!-- Logout Modal-->
    <div class="modal fade" id="logoutModal" tabindex="-1" role="dialog" aria-labelledby="logoutModalLabel" aria-hidden="true">
      <div class="modal-dialog" role="document">
        <div class="modal-content">
          <div class="modal-header">
            <h5 class="modal-title" id="exampleModalLabel">Ready to Leave?</h5>
            <button class="close" type="button" data-dismiss="modal" aria-label="Close">
              <span aria-hidden="true">×</span>
            </button>
          </div>
          <div class="modal-body">Select "Logout" below if you are ready to end your current session.</div>
          <div class="modal-footer">
            <button class="btn btn-secondary" type="button" data-dismiss="modal">Cancel</button>
            <router-link class="nav-link" to="/login" data-dismiss="modal">
              <a class="btn btn-primary" href="login.html">Logout</a>
            </router-link>
          </div>
        </div>
      </div>
    </div>
    <div v-if="!account.status.loggedIn">
      <Alert v-if="alert.message" :class="`alert ${alert.type}`" />
      <router-view></router-view>
    </div>
  </div>
</template>

<script>
import { mapState, mapActions } from "vuex";
import Alert from "./components/alert/Alert";

export default {
  name: "app-root",
  data: function() {
    return {
      isSidebarToggled: false
    };
  },
  computed: {
    ...mapState({
      alert: state => state.alert,
      account: state => state.account
    })
  },
  methods: {
    ...mapActions({
      clearAlert: "alert/clear"
    }),
    sidebarToggler: function() {
      this.isSidebarToggled = !this.isSidebarToggled;
    }
  },
  watch: {
    $route(to, from) {
      // clear alert on location change
      this.clearAlert();
    }
  },
  components: {
    Alert
  }
};
</script>
