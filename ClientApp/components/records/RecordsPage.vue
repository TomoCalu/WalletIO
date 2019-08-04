<template>
    <!-- Begin Page Content -->
    <div class="container-fluid">

        <div class="d-sm-flex align-items-center justify-content-between mb-4">
            <h1 class="h3 mb-0 text-gray-800">Records</h1>
            <a href="#" class="d-none d-sm-inline-block btn btn-sm btn-primary shadow-sm" data-toggle="modal" data-target="#newRecordModal">
                <i class="fas fa-download fa-sm text-white-50"></i> Add a new record
            </a>
            </div>
        
        <!-- DataTales Example -->
        <div class="card shadow mb-4">
            <div class="card-header py-3">
            <h6 class="m-0 font-weight-bold text-primary">Records table</h6>
            </div>
            <div class="card-body">
            <div class="table-responsive">
                <table class="table table-bordered" id="dataTable" width="100%" cellspacing="0">
                <thead>
                    <tr>
                        <th>Account</th>
                        <th>Entry type</th>
                        <th>Category</th>
                        <th>Description</th>
                        <th>Money amount</th>
                    </tr>
                </thead>
                <tbody>
                    <tr v-for="record in records" v-bind:key="record.id">
                        <td>{{record.account.name}}</td>
                        <td>{{record.entryType.name}}</td>
                        <td>{{record.category.name}}</td>
                        <td>{{record.description}}</td>
                        <td>{{record.moneyAmount}}</td>
                    </tr>
                </tbody>
                </table>
            </div>
        </div>
    </div>
    <!-- Add new account modal-->
        <div class="modal fade" id="newRecordModal" tabindex="-1" role="dialog" aria-labelledby="newRecordModalLabel" aria-hidden="true">
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
                            <form v-on:submit="handleSubmitRecord" class="newRecord">
                                <!--<Alert v-if="alert.message" :class="`alert ${alert.type}`" />-->
                                <div class="form-group row">
                                    <div class="col-sm-6 mb-3 mb-sm-0">
                                        <label for="accountDropdown">Account</label>
                                        <select class="form-control" v-model="selectedAccount" name="account" v-validate="'required'" :class="{ 'is-invalid': submitted && errors.has('account')}">
                                            <option class="btn btn-default" v-for="account in accounts" v-bind:key="account.id">{{account.name}}</option>
                                        </select>
                                        <span v-show="errors.has('account')" class="text-danger">{{ errors.first('account') }}</span>
                                    </div>
                                    <div class="col-sm-6 mb-3 mb-sm-0">                                        
                                    </div>
                                    <div class="col-sm-6 mb-3 mb-sm-0">
                                        <label for="entryTypeDropdown">Entry type</label>
                                        <select class="form-control" v-model="selectedEntryType" name="entryType" v-validate="'required'" :class="{ 'is-invalid': submitted && errors.has('entryType')}">
                                            <option class="btn btn-default" v-for="entryType in entryTypes" v-bind:key="entryType.id">{{entryType.name}}</option>
                                        </select>
                                        <span v-show="errors.has('entryType')" class="text-danger">{{ errors.first('entryType') }}</span>
                                    </div>
                                    <div class="col-sm-6 mb-3 mb-sm-0">
                                        <label for="categoryDropdown">Category</label>
                                        <select class="form-control" v-model="selectedCategory" name="category" v-validate="'required'" :class="{ 'is-invalid': submitted && errors.has('category')}">
                                            <option class="btn btn-default" v-for="category in categories" v-bind:key="category.id">{{category.name}}</option>
                                        </select>
                                        <span v-show="errors.has('category')" class="text-danger">{{ errors.first('category') }}</span>
                                    </div>
                                    <div class="col-sm-6 mb-3 mb-sm-0">
                                        <label for="name">Description</label>
                                        <input type="text" v-model="newRecord.description" v-validate="'required'" name="name" class="form-control" :class="{ 'is-invalid': submitted && errors.has('name') }" />
                                        <div v-if="submitted && errors.has('name')" class="invalid-feedback">{{ errors.first('name') }}</div>
                                    </div>
                                    <div class="col-sm-6 mb-3 mb-sm-0">
                                        <label for="moneyAmount">Value (KN)</label>
                                        <input type="number" v-model="newRecord.moneyAmount" v-validate="'required'" name="moneyAmount" class="form-control" :class="{ 'is-invalid': submitted && errors.has('moneyAmount') }" />
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

    </div>
    <!-- /.container-fluid -->
</template>

<script>
import { mapState, mapActions } from "vuex";
import { entryTypeService, accountService, recordService } from '../_services';

export default {
    data(){
        return {
            pageNumber: 0,  // default to page 0,
            records: [],
            accounts: [],
            entryTypes: [],
            categories: [],
            selectedAccount: '', 
            selectedEntryType: '', 
            selectedCategory: '',
            newRecord: {
                accountId: '',
                categoryId: '',
                description: '',
                moneyAmount:''
            },
            submitted: false
        }
    },
    computed: {
        ...mapState({
            alert: state => state.alert,
            account: state => state.account
        })
    },
    methods: {
        addNewRecord(){
            this.getAccountId();
            this.getCategoryId();
            recordService.addNew(this.newRecord);
        },
        handleSubmitRecord(e) {
            this.submitted = true;
            this.$validator.validate().then(async (valid) => {
                if (valid) {
                    e.preventDefault();
                    this.addNewRecord();
                    location.reload();
                }
                else {
                    e.preventDefault();
                    return;
                } 
            });
        },
        getCategories() {
            for(var i=0; i<this.entryTypes.length; i++)
            {
                if(this.entryTypes[i].name == this.selectedEntryType)
                {
                    this.categories = this.entryTypes[i].categories;
                    return;
                }
            }
            this.categories = null;
            return;
        },
        getEntryTypesWithCategories() {            
            entryTypeService.getWithCategories().then(response => {            
                this.entryTypes = response;
            });
        },
        getAllAccountsForUser() {
            accountService.getByIdUser(this.account.user.id).then(response => {            
                this.accounts = response;
            });
        },
        getAllRecordsForUser(){
            var finished = recordService.getByIdUser(this.account.user.id).then(response => {
                                this.records = response;
                            });
            return finished;

        },
        getAccountId(){
            for(var i=0; i<this.accounts.length; i++)
            {
                if(this.accounts[i].name == this.selectedAccount)
                {
                    this.newRecord.accountId = this.accounts[i].id;
                    return;
                }
            }
            this.newRecord.accountId = null;
            return;
        },
        getCategoryId(){
            for(var i=0; i<this.categories.length; i++)
            {
                if(this.categories[i].name == this.selectedCategory)
                {
                    this.newRecord.categoryId = this.categories[i].id;
                    return;
                }
            }
            this.newRecord.accountId = null;
            return;
        }
    },
    created: async function(){
        this.getEntryTypesWithCategories();
        this.getAllAccountsForUser();
        await this.getAllRecordsForUser();
    },
    watch: {
        selectedEntryType: function (newEntryType, oldEntryType) {
            this.getCategories();
            this.selectedCategory = "";
        }
    }
};
</script>