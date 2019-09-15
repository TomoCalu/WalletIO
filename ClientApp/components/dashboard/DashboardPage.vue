<template>
  <!-- Begin Page Content -->
  <div class="container-fluid">
    <!-- Page Heading -->
    <div class="d-sm-flex align-items-center justify-content-between mb-4">
      <h1 class="h3 mb-0 text-gray-800">Dashboard</h1>
      <a href="#" class="d-none d-inline-block btn btn-sm btn-primary shadow-sm" data-toggle="modal" data-target="#newAccountModal" v-if="accounts.length < 8" v-on:click="clearNewAccountData()">
        <i class="fas fa-plus fa-sm text-white-50"></i> Add new cash or bank account
      </a>
    </div>

    <!-- Content Row -->
    <div class="row">      
      <h2 class="h5 mb-0 text-gray-800" v-if="checkIfAnyAccountsExist() && accountsFetched">Start by adding a new account in the top right corner. </h2>
      <!-- Account tabs -->
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
              <div class="col-auto hover-cursor"  v-on:click="toggleSelectedAccount(account.id)">
                <i class="fas fa-dollar-sign fa-2x" v-bind:class="[checkIfSelected(account.id)? 'selected-account-colour' : 'text-gray-300']"></i>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>

    <!-- Content Row -->

    <div class="row">
      <!-- Area Chart -->
      <div class="col-xl-8 col-lg-7" v-if="accounts.length > 0">
        <div class="card shadow mb-4">
          <!-- Card Header - Dropdown -->
          <div class="card-header py-3 d-flex flex-row align-items-center justify-content-between">
            <h6 class="m-0 font-weight-bold text-primary">Spendings and income trends ({{selectedBalanceDataRange}})</h6>
            <div class="dropdown no-arrow">
              <a class="dropdown-toggle" href="#" role="button" id="dropdownMenuLink" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                <i class="fas fa-ellipsis-v fa-sm fa-fw text-gray-400"></i>
              </a>
              <div class="dropdown-menu dropdown-menu-right shadow animated--fade-in" aria-labelledby="dropdownMenuLink">
                <div class="dropdown-header">Select range:</div>
                <div v-for="choice in balanceDataChoices" v-bind:key="choice.id">
                  <button class="dropdown-item hover-cursor" v-on:click="selectedBalanceDataRange = choice">{{choice}}</button>
                </div>
              </div>
            </div>
          </div>
          <!-- Card Body -->
          <div class="card-body">
            <div class="chart-area">
              <canvas id="balanceTrendsChart"></canvas>
            </div>
          </div>
        </div>
      </div>

      <!-- Pie Chart -->
      <div class="col-xl-4 col-lg-5" v-if="accounts.length > 0">
        <div class="card shadow mb-4">
          <!-- Card Header - Dropdown -->
          <div class="card-header py-3 d-flex flex-row align-items-center justify-content-between">
            <h6 class="m-0 font-weight-bold text-primary">Expenses structure ({{selectedExpensesStructureDataRange}})</h6>
            <div class="dropdown no-arrow">
              <a class="dropdown-toggle" href="#" role="button" id="dropdownMenuLink" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                <i class="fas fa-ellipsis-v fa-sm fa-fw text-gray-400"></i>
              </a>
              <div class="dropdown-menu dropdown-menu-right shadow animated--fade-in" aria-labelledby="dropdownMenuLink">
                <div class="dropdown-header">Select range:</div>
                <div v-for="choice in expensesStructureDataChoices" v-bind:key="choice.id">
                  <button class="dropdown-item hover-cursor" v-on:click="selectedExpensesStructureDataRange = choice">{{choice}}</button>
                </div>
              </div>
            </div>
          </div>
          <!-- Card Body -->
          <div class="card-body" style="height:360px">
            <div class="chart-pie pt-6 pb-2">
              <canvas id="allExpensesChart"></canvas>
            </div>
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
                    <input type="number" v-model="newAccount.moneyAmount" v-validate="{required, regex: /^[1-9]\d*$/}" name="moneyAmount" class="form-control" :class="{ 'is-invalid': submitted && errors.has('moneyAmount') }" />
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
                    <input type="number" v-model="newAccount.moneyAmount" v-validate="{required, regex: /^[1-9]\d*$/}" name="moneyAmount" class="form-control" :class="{ 'is-invalid': submitted && errors.has('moneyAmount') }" />
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
import { accountService, recordService } from '../_services';
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
      accountsFetched: false,
      submitted: false,
      accountOptions: [],
      idAccountToDelete: '',
      expensesStructureData: '',
      expensesStructureDataChoices: ['Last 7 days', 'Last 30 days', 'Last year', 'All'],
      selectedExpensesStructureDataRange: 'All',
      balanceTrendsData: '',
      balanceDataChoices: ['Week', 'Month', 'Year'],
      selectedBalanceDataRange: 'Week',
      selectedAccounts: [],
      selectedAccountsString: '',
      allExpensesChart: {
        type: 'pie',
        data: {
          datasets: [
            {
              backgroundColor: ["#eb4034", "#1cd1ed","#edce1c","#707070","#bc18d9", "#2bd918", "#4568f5", "#1cebb4", "#f00a97", "#d4d4d4"],
              data: [],
              borderWidth: 0,
            }
          ],
        },
        options: {
          maintainAspectRatio: false,
          tooltips: {
            backgroundColor: "rgb(255,255,255)",
            bodyFontColor: "#858796",
            borderColor: '#dddfeb',
            borderWidth: 1,
            xPadding: 15,
            yPadding: 15,
            displayColors: false,
            caretPadding: 10,
          },
          legend: {position:'bottom', 
            labels:{pointStyle:'circle',
                    usePointStyle:true}
          },
          cutoutPercentage: 85
        }
      },
      balanceTrendsChart : {
        type: 'line',
        data: {
          datasets: [
            {
              borderColor: ["#30fc03"],
              label: "Income (kuna)",
              data: []
            },
            {
              borderColor: ["#fc2003"],
              label: "Spendings (kuna)",
              data: []
            }
          ],
        },
        options: {
          responsive: true,
          maintainAspectRatio: false,
          scales: {
            yAxes: [{
              ticks: {
                beginAtZero:true
              }
            }]
          }
        }
      }
    }
  },
  computed: {
    ...mapState({
      alert: state => state.alert,
      userInfo: state => state.userInfo
    })
  },
  methods: {
    getAllAccountsForUser() {
      var finished = accountService.getByIdUser(this.userInfo.user.id).then(response => {            
        this.accounts = response;
      });
      return finished;
    },
    addNewAccount() {
      this.newAccount.userId = this.userInfo.user.id;
      var finished = accountService.addNew(this.newAccount);
      return finished;
    },
    updateAccount() {
      this.newAccount.userId = this.userInfo.user.id;
      accountService.update(this.newAccount);
    },
    deleteAccount() {
      accountService.delete(this.idAccountToDelete);
      window.location.reload()
    },
    async getExpensesStructure() {
      if (this.selectedAccounts.length > 0) {
        this.selectedAccounts.forEach(selectedAccount => {
          this.selectedAccountsString = this.selectedAccountsString.concat('selectedAccounts=',selectedAccount,'&');
        });
        this.selectedAccountsString  = this.selectedAccountsString.substring(0, this.selectedAccountsString.length - 1);
        await recordService.getSpendingsSums(this.userInfo.user.id, this.selectedExpensesStructureDataRange, this.selectedAccountsString).then(response => {
          this.expensesStructureData = response;
        });
        this.selectedAccountsString='';        
      }
      else {        
        await recordService.getSpendingsSums(this.userInfo.user.id, this.selectedExpensesStructureDataRange, null).then(response => {
          this.expensesStructureData = response;
        });
      }
      this.allExpensesChart.data.datasets[0].data = this.expensesStructureData.value;
      this.allExpensesChart.data.labels = this.expensesStructureData.key;
    },
    async getBalanceTrendsStructure() { 
      if (this.selectedAccounts.length > 0) {
        this.selectedAccounts.forEach(selectedAccount => {
          this.selectedAccountsString = this.selectedAccountsString.concat('selectedAccounts=',selectedAccount,'&');
        });
        this.selectedAccountsString  = this.selectedAccountsString.substring(0, this.selectedAccountsString.length - 1);
        await recordService.getBalanceTrends(this.userInfo.user.id, this.selectedBalanceDataRange, this.selectedAccountsString).then(response => {
          this.balanceTrendsData = response;
        });
        this.selectedAccountsString='';
      }
      else {        
        await recordService.getBalanceTrends(this.userInfo.user.id, this.selectedBalanceDataRange, null).then(response => {
          this.balanceTrendsData = response;
        });
      }
      this.balanceTrendsChart.data.datasets[0].data = this.balanceTrendsData.item2;
      this.balanceTrendsChart.data.datasets[1].data = this.balanceTrendsData.item3;
      this.balanceTrendsChart.data.labels = this.balanceTrendsData.item1;
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
        if(this.accounts[i].id == this.newAccount.id) {
          return true;
        } 
      }
      return false;
    },
    createChart(chartId, chartData) {
      const ctx = document.getElementById(chartId);
      const myChart = new Chart(ctx, {
        type: chartData.type,        
        data: chartData.data,
        options: chartData.options
      });
    },
    toggleSelectedAccount(accountId) {
      for(var i=0; i<this.selectedAccounts.length; i++) {
        if(this.selectedAccounts[i] == accountId) {
          this.selectedAccounts.splice(i,1);
          return;
        }
      }
      this.selectedAccounts.push(accountId)
    },
    checkIfSelected(accountId){
      if(this.selectedAccounts.length == 0) return false;
      for(var i=0; i<this.selectedAccounts.length; i++)
      {
        if(this.selectedAccounts[i] == accountId) return true;
      }
      return false;
    },
    checkIfAnyAccountsExist(){
      if(this.accounts.length == 0) return true;
      else false;
    },
    clearNewAccountData() {
      this.newAccount.name = "";
      this.newAccount.moneyAmount = "";
      delete this.newAccount.id;
      delete this.newAccount.createdTimestamp;
      delete this.newAccount.records;
      this.selectedAccount = "";
      this.selectedEntryType = "";
      this.selectedCategory = "";
      this.$validator.reset();
    }
  },
  created: async function(){
    await this.getAllAccountsForUser();
    this.accountsFetched = true;
    await this.getExpensesStructure();
    await this.getBalanceTrendsStructure();
    for (var i=0; i<this.accounts.length; i++) { 
      this.accountOptions[i] = false;
    }
    this.createChart('balanceTrendsChart', this.balanceTrendsChart);
    this.createChart('allExpensesChart', this.allExpensesChart);
  },
  watch: {
    selectedBalanceDataRange: async function (newRange, oldRange) {
      await this.getBalanceTrendsStructure();
      this.createChart('balanceTrendsChart', this.balanceTrendsChart);
    },
    selectedExpensesStructureDataRange: async function (newRange, oldRange) {
      await this.getExpensesStructure();
      this.createChart('allExpensesChart', this.allExpensesChart);
    },
    selectedAccounts: async function (newList, oldList) {
      await this.getBalanceTrendsStructure();
      await this.getExpensesStructure();
      this.createChart('balanceTrendsChart', this.balanceTrendsChart);
      this.createChart('allExpensesChart', this.allExpensesChart); 
    }
  },
  components: {
    Alert
  }
};

</script>

