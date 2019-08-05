<template>
  <!-- Begin Page Content -->
  <div class="container-fluid">
    <!-- Page Heading -->
    <div class="d-sm-flex align-items-center justify-content-between mb-4">
      <h1 class="h3 mb-0 text-gray-800">Dashboard</h1>
      <a href="#" class="d-none d-sm-inline-block btn btn-sm btn-primary shadow-sm" data-toggle="modal" data-target="#newAccountModal">
        <i class="fas fa-download fa-sm text-white-50"></i> Add new cash or bank account
      </a>
    </div>

    <!-- Content Row -->
    <div class="row">
      <!-- Earnings (Monthly) Card Example -->
      <div class="col-xl-3 col-md-6 mb-4" v-for="(account, index) in accounts" v-bind:key="account.id">
        <div class="card border-left-success shadow h-100 py-2" v-on:mouseover="showAccountOptions(index)" v-on:mouseleave="hideAccountOptions(index)">
          <div class="card-body">
            <div class="row no-gutters align-items-center d-flex justify-content-end" v-if="accountOptions[index]">
              <i class="fas fa-pencil-alt hover-account-options" v-on:click="getCurrentAccount(index)" data-toggle="modal" data-target="#editAccountModal"></i>
              <i class="fas fa-trash-alt ml-2 hover-account-options" v-on:click="idAccountToDelete = account.id" data-toggle="modal" data-target="#deleteAccountModal"></i>
            </div>
            <div class="row no-gutters align-items-center d-flex justify-content-end" v-else-if="!accountOptions[index]" style="max-height:16px;">
              &nbsp;
            </div>
            <div class="row no-gutters align-items-center">
              <div class="col mr-2">
                <div
                  class="text-xs font-weight-bold text-success text-uppercase mb-1">{{account.name}}</div>
                <div class="h5 mb-0 font-weight-bold text-gray-800">{{account.moneyAmount}} KN</div>
              </div>
              <div class="col-auto">
                <i class="fas fa-dollar-sign fa-2x text-gray-300"></i>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>

    <!-- Content Row -->

    <div class="row">
      <!-- Area Chart -->
      <div class="col-xl-8 col-lg-7">
        <div class="card shadow mb-4">
          <!-- Card Header - Dropdown -->
          <div class="card-header py-3 d-flex flex-row align-items-center justify-content-between">
            <h6 class="m-0 font-weight-bold text-primary">Earnings Overview</h6>
            <div class="dropdown no-arrow">
              <a
                class="dropdown-toggle"
                href="#"
                role="button"
                id="dropdownMenuLink"
                data-toggle="dropdown"
                aria-haspopup="true"
                aria-expanded="false"
              >
                <i class="fas fa-ellipsis-v fa-sm fa-fw text-gray-400"></i>
              </a>
              <div
                class="dropdown-menu dropdown-menu-right shadow animated--fade-in"
                aria-labelledby="dropdownMenuLink"
              >
                <div class="dropdown-header">Dropdown Header:</div>
                <a class="dropdown-item" href="#">Action</a>
                <a class="dropdown-item" href="#">Another action</a>
                <div class="dropdown-divider"></div>
                <a class="dropdown-item" href="#">Something else here</a>
              </div>
            </div>
          </div>
          <!-- Card Body -->
          <div class="card-body">
            <div class="chart-area">
              <canvas id="myAreaChart"></canvas>
            </div>
          </div>
        </div>
      </div>

      <!-- Pie Chart -->
      <div class="col-xl-4 col-lg-5">
        <div class="card shadow mb-4">
          <!-- Card Header - Dropdown -->
          <div class="card-header py-3 d-flex flex-row align-items-center justify-content-between">
            <h6 class="m-0 font-weight-bold text-primary">Revenue Sources</h6>
            <div class="dropdown no-arrow">
              <a
                class="dropdown-toggle"
                href="#"
                role="button"
                id="dropdownMenuLink"
                data-toggle="dropdown"
                aria-haspopup="true"
                aria-expanded="false"
              >
                <i class="fas fa-ellipsis-v fa-sm fa-fw text-gray-400"></i>
              </a>
              <div
                class="dropdown-menu dropdown-menu-right shadow animated--fade-in"
                aria-labelledby="dropdownMenuLink"
              >
                <div class="dropdown-header">Dropdown Header:</div>
                <a class="dropdown-item" href="#">Action</a>
                <a class="dropdown-item" href="#">Another action</a>
                <div class="dropdown-divider"></div>
                <a class="dropdown-item" href="#">Something else here</a>
              </div>
            </div>
          </div>
          <!-- Card Body -->
          <div class="card-body">
            <div class="chart-pie pt-4 pb-2">
              <canvas id="myPieChart"></canvas>
            </div>
            <div class="mt-4 text-center small">
              <span class="mr-2">
                <i class="fas fa-circle text-primary"></i> Direct
              </span>
              <span class="mr-2">
                <i class="fas fa-circle text-success"></i> Social
              </span>
              <span class="mr-2">
                <i class="fas fa-circle text-info"></i> Referral
              </span>
            </div>
          </div>
        </div>
      </div>
    </div>

    <!-- Content Row -->
    <div class="row">
      <!-- Content Column -->
      <div class="col-lg-6 mb-4">
        <!-- Project Card Example -->
        <div class="card shadow mb-4">
          <div class="card-header py-3">
            <h6 class="m-0 font-weight-bold text-primary">Projects</h6>
          </div>
          <div class="card-body">
            <h4 class="small font-weight-bold">
              Server Migration
              <span class="float-right">20%</span>
            </h4>
            <div class="progress mb-4">
              <div
                class="progress-bar bg-danger"
                role="progressbar"
                style="width: 20%"
                aria-valuenow="20"
                aria-valuemin="0"
                aria-valuemax="100"
              ></div>
            </div>
            <h4 class="small font-weight-bold">
              Sales Tracking
              <span class="float-right">40%</span>
            </h4>
            <div class="progress mb-4">
              <div
                class="progress-bar bg-warning"
                role="progressbar"
                style="width: 40%"
                aria-valuenow="40"
                aria-valuemin="0"
                aria-valuemax="100"
              ></div>
            </div>
            <h4 class="small font-weight-bold">
              Customer Database
              <span class="float-right">60%</span>
            </h4>
            <div class="progress mb-4">
              <div
                class="progress-bar"
                role="progressbar"
                style="width: 60%"
                aria-valuenow="60"
                aria-valuemin="0"
                aria-valuemax="100"
              ></div>
            </div>
            <h4 class="small font-weight-bold">
              Payout Details
              <span class="float-right">80%</span>
            </h4>
            <div class="progress mb-4">
              <div
                class="progress-bar bg-info"
                role="progressbar"
                style="width: 80%"
                aria-valuenow="80"
                aria-valuemin="0"
                aria-valuemax="100"
              ></div>
            </div>
            <h4 class="small font-weight-bold">
              Account Setup
              <span class="float-right">Complete!</span>
            </h4>
            <div class="progress">
              <div
                class="progress-bar bg-success"
                role="progressbar"
                style="width: 100%"
                aria-valuenow="100"
                aria-valuemin="0"
                aria-valuemax="100"
              ></div>
            </div>
          </div>
        </div>

        <!-- Color System -->
        <div class="row">
          <div class="col-lg-6 mb-4">
            <div class="card bg-primary text-white shadow">
              <div class="card-body">
                Primary
                <div class="text-white-50 small">#4e73df</div>
              </div>
            </div>
          </div>
          <div class="col-lg-6 mb-4">
            <div class="card bg-success text-white shadow">
              <div class="card-body">
                Success
                <div class="text-white-50 small">#1cc88a</div>
              </div>
            </div>
          </div>
          <div class="col-lg-6 mb-4">
            <div class="card bg-info text-white shadow">
              <div class="card-body">
                Info
                <div class="text-white-50 small">#36b9cc</div>
              </div>
            </div>
          </div>
          <div class="col-lg-6 mb-4">
            <div class="card bg-warning text-white shadow">
              <div class="card-body">
                Warning
                <div class="text-white-50 small">#f6c23e</div>
              </div>
            </div>
          </div>
          <div class="col-lg-6 mb-4">
            <div class="card bg-danger text-white shadow">
              <div class="card-body">
                Danger
                <div class="text-white-50 small">#e74a3b</div>
              </div>
            </div>
          </div>
          <div class="col-lg-6 mb-4">
            <div class="card bg-secondary text-white shadow">
              <div class="card-body">
                Secondary
                <div class="text-white-50 small">#858796</div>
              </div>
            </div>
          </div>
        </div>
      </div>

      <div class="col-lg-6 mb-4">
        <!-- Illustrations -->
        <div class="card shadow mb-4">
          <div class="card-header py-3">
            <h6 class="m-0 font-weight-bold text-primary">Illustrations</h6>
          </div>
          <div class="card-body">
            <div class="text-center">
              <img
                class="img-fluid px-3 px-sm-4 mt-3 mb-4"
                style="width: 25rem;"
                src="img/undraw_posting_photo.svg"
                alt
              />
            </div>
            <p>
              Add some quality, svg illustrations to your project courtesy of
              <a target="_blank" rel="nofollow" href="https://undraw.co/">unDraw</a>, a constantly updated collection of beautiful svg images that you can use completely free and without attribution!
            </p>
            <a
              target="_blank"
              rel="nofollow"
              href="https://undraw.co/"
            >Browse Illustrations on unDraw &rarr;</a>
          </div>
        </div>

        <!-- Approach -->
        <div class="card shadow mb-4">
          <div class="card-header py-3">
            <h6 class="m-0 font-weight-bold text-primary">Development Approach</h6>
          </div>
          <div class="card-body">
            <p>SB Admin 2 makes extensive use of Bootstrap 4 utility classes in order to reduce CSS bloat and poor page performance. Custom CSS classes are used to create custom components and custom utility classes.</p>
            <p
              class="mb-0"
            >Before working with this theme, you should become familiar with the Bootstrap framework, especially the utility classes.</p>
          </div>
        </div>
      </div>
    </div>  

    <!-- Add new account modal-->
    <div class="modal fade" id="newAccountModal" tabindex="-1" role="dialog" aria-labelledby="newAccountModalLabel" aria-hidden="true">
      <div class="modal-dialog" role="document">
        <div class="modal-content">
          <div class="modal-header">
            <h5 class="modal-title" id="exampleModalLabel">Add a new account</h5>
            <button class="close" type="button" data-dismiss="modal" aria-label="Close">
              <span aria-hidden="true">×</span>
            </button>
          </div>
          <div class="modal-body">         
            <div>
              <form v-on:submit="handleSubmitAccount" class="newAccount">
                <Alert v-if="alert.message" :class="`alert ${alert.type}`" />
                <div class="form-group row">
                  <div class="col-sm-6 mb-3 mb-sm-0">
                    <label for="name">Account Name</label>
                    <input type="text" v-model="newAccount.name" v-validate="'required'" name="name" class="form-control" :class="{ 'is-invalid': submitted && errors.has('name') }" />
                    <div v-if="submitted && errors.has('name')" class="invalid-feedback">{{ errors.first('name') }}</div>
                  </div>
                  <div class="col-sm-6 mb-3 mb-sm-0">
                    <label for="moneyAmount">Starting money amount (KN)</label>
                    <input type="number" v-model="newAccount.moneyAmount" v-validate="'required'" name="moneyAmount" class="form-control" :class="{ 'is-invalid': submitted && errors.has('moneyAmount') }" />
                    <div v-if="submitted && errors.has('moneyAmount')" class="invalid-feedback">{{ errors.first('moneyAmount') }}</div>
                  </div>
                </div>                
                <div class="modal-footer">
                  <button class="btn btn-secondary" type="button" data-dismiss="modal">Cancel</button>
                  <button class="btn btn-primary" type="submit">Accept</button>
                </div>
              </form>
            </div>
          </div>
        </div>
      </div>
    </div>

    <!-- Edit account modal-->
    <div class="modal fade" id="editAccountModal" tabindex="-1" role="dialog" aria-labelledby="editAccountModalLabel" aria-hidden="true">
      <div class="modal-dialog" role="document">
        <div class="modal-content">
          <div class="modal-header">
            <h5 class="modal-title" id="exampleModalLabel">Edit account</h5>
            <button class="close" type="button" data-dismiss="modal" aria-label="Close">
              <span aria-hidden="true">×</span>
            </button>
          </div>
          <div class="modal-body">         
            <div>
              <form v-on:submit="handleSubmitAccount" class="newAccount">
                <Alert v-if="alert.message" :class="`alert ${alert.type}`" />
                <div class="form-group row">
                  <div class="col-sm-6 mb-3 mb-sm-0">
                    <label for="name">Account Name</label>
                    <input type="text" v-model="newAccount.name" v-validate="'required'" name="name" class="form-control" :class="{ 'is-invalid': submitted && errors.has('name') }" />
                    <div v-if="submitted && errors.has('name')" class="invalid-feedback">{{ errors.first('name') }}</div>
                  </div>
                  <div class="col-sm-6 mb-3 mb-sm-0">
                    <label for="moneyAmount">Starting money amount (KN)</label>
                    <input type="number" v-model="newAccount.moneyAmount" v-validate="'required'" name="moneyAmount" class="form-control" :class="{ 'is-invalid': submitted && errors.has('moneyAmount') }" />
                    <div v-if="submitted && errors.has('moneyAmount')" class="invalid-feedback">{{ errors.first('moneyAmount') }}</div>
                  </div>
                </div>                
                <div class="modal-footer">
                  <button class="btn btn-secondary" type="button" data-dismiss="modal">Cancel</button>
                  <button class="btn btn-primary" type="submit">Accept</button>
                </div>
              </form>
            </div>
          </div>
        </div>
      </div>
    </div>

    <!-- Delete account Modal-->
    <div class="modal fade" id="deleteAccountModal" tabindex="-1" role="dialog" aria-labelledby="deleteAccountModalLabel" aria-hidden="true">
      <div class="modal-dialog" role="document">
        <div class="modal-content">
          <div class="modal-header">
            <h5 class="modal-title" id="exampleModalLabel">Are you sure you want to delete this account?</h5>
            <button class="close" type="button" data-dismiss="modal" aria-label="Close">
              <span aria-hidden="true">×</span>
            </button>
          </div>
          <div class="modal-body">Select "Delete" below if you are ready to delete this account.</div>
          <div class="modal-footer">
            <button class="btn btn-secondary" type="button" data-dismiss="modal">Cancel</button>
            <button class="btn btn-danger" v-on:click="deleteAccount">Delete</button>
          </div>
        </div>
      </div>
    </div>
  </div>
  <!-- /.container-fluid -->
</template>

<script>
import { mapState, mapActions } from "vuex";
import { accountService } from '../_services';
import Alert from "../alert/Alert";

export default {
  data: function() {
    return {
      accounts: [],
      newAccount: {
        name: '',
        moneyAmount:'',
        userId: ''
      },
      submitted: false,
      accountOptions: [],
      idAccountToDelete: ''
    };
  },
  computed: {
    ...mapState({
      alert: state => state.alert,
      account: state => state.account
    })
  },
  methods: {
    getAllAccountsForUser() {
      var finished = accountService.getByIdUser(this.account.user.id).then(response => {            
        this.accounts = response;
      });
      return finished;
    },
    addNewAccount() {
      this.newAccount.userId = this.account.user.id;
      var finished = accountService.addNew(this.newAccount);
      return finished;
    },
    updateAccount() {
      this.newAccount.userId = this.account.user.id;
      accountService.update(this.newAccount);
    },
    deleteAccount() {
      accountService.delete(this.idAccountToDelete);
      window.location.reload()
    },
    handleSubmitAccount(e) {
      this.submitted = true;
      this.$validator.validate().then(async (valid) => {
        if (valid) {
          e.preventDefault();
          if(this.checkIfAccountExists() == true) {
            this.updateAccount();  
          }
          else {
            await this.addNewAccount();            
          }
          var that = this; 
            
          setTimeout(function(){
            if (String(that.$store.state.alert.type) == "alert-danger") {
              return;
            }
            else location.reload();
          }, 1000);
        }
        else {
          e.preventDefault();
          return;
        } 
      });
    },
    showAccountOptions(accountId) {
      this.$set(this.accountOptions, accountId, true)
    },
    hideAccountOptions(accountId) {
      this.$set(this.accountOptions, accountId, false)
    },
    getCurrentAccount(index){
      this.newAccount = {...this.accounts[index]};
    },
    checkIfAccountExists() {
      if(this.accounts.length == 0) return false;
      for(var i=0; i < this.accounts.length; i++)
      {
        if(this.accounts[i].id == this.newAccount.id){
          return true;
        } 
      }
      return false;
    }
  },
  created: async function(){
    await this.getAllAccountsForUser();
    for (var i=0; i<this.accounts.length; i++) { 
      this.accountOptions[i] = false;
    }
  },
  components: {
    Alert
  }
};

</script>

