<template>
    <!-- Begin Page Content -->
    <div class="container-fluid"  v-if="accounts.length > 0">

        <div class="d-sm-flex align-items-center justify-content-between mb-4">
            <h1 class="h3 mb-0 text-gray-800">Records</h1>
            <a href="#" class="d-none d-inline-block btn btn-sm btn-primary shadow-sm" data-toggle="modal" data-target="#newRecordModal"  v-on:click="clearNewRecordData()">
                <i class="fas fa-plus fa-sm text-white-50"></i> Add a new record
            </a>
        </div>

        <!-- DataTable containing records -->
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
                                <th>Category</th>
                                <th>Description</th>
                                <th>Money amount</th>
                                <th>Options</th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr v-for="(record, index) in records" v-bind:key="record.id">
                                <td>{{record.account.name}}</td>
                                <td>{{record.category.name}}</td>
                                <td>{{record.description}}</td>
                                <td>
                                    <p v-if="checkIfCategoryIsIncome(record.category)" class="text-success">+{{record.moneyAmount}} kn </p>
                                    <p v-else class="text-danger">-{{record.moneyAmount}} kn</p>
                                </td>
                                
                                <td class="d-flex justify-content-around">
                                    <div class="btn btn-primary hover-cursor" v-on:click="getCurrentRecord(index)" data-toggle="modal" data-target="#editRecordModal">
                                        <i class="fas fa-pencil-alt" > Edit</i>
                                    </div>
                                    <div class="btn btn-danger hover-cursor" v-on:click="idRecordToDelete = record.id" data-toggle="modal" data-target="#deleteRecordModal">
                                        <i class="fas fa-trash-alt"> Delete</i>
                                    </div>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
            </div>
        </div>
        
        <!-- Add new record modal-->
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
                                        <input type="text" v-model="newRecord.description" v-validate="'required'" name="description" class="form-control" :class="{ 'is-invalid': submitted && errors.has('description') }" />
                                        <div v-if="submitted && errors.has('description')" class="invalid-feedback">{{ errors.first('description') }}</div>
                                    </div>
                                    <div class="col-sm-6 mb-3 mb-sm-0">
                                        <label for="moneyAmount">Value (KN)</label>
                                        <input type="number" v-model="newRecord.moneyAmount" v-validate="{required, regex: /^[1-9]\d*$/}" name="moneyAmount" class="form-control" :class="{ 'is-invalid': submitted && errors.has('moneyAmount') }" />
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

        <!-- Edit record modal-->
        <div class="modal fade" id="editRecordModal" tabindex="-1" role="dialog" aria-labelledby="editRecordModalLabel" aria-hidden="true">
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
                                        <input type="text" v-model="newRecord.description" v-validate="'required'" name="description" class="form-control" :class="{ 'is-invalid': submitted && errors.has('description') }" />
                                        <div v-if="submitted && errors.has('description')" class="invalid-feedback">{{ errors.first('description') }}</div>
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

        <!-- Delete record Modal-->
        <div class="modal fade" id="deleteRecordModal" tabindex="-1" role="dialog" aria-labelledby="deleteRecordModalLabel" aria-hidden="true">
            <div class="modal-dialog" role="document">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title" id="exampleModalLabel">Are you sure you want to delete this record?</h5>
                        <button class="close" type="button" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">×</span>
                        </button>
                    </div>
                    <div class="modal-body">Select "Delete" below if you are ready to delete this record.</div>
                    <div class="modal-footer">
                        <button class="btn btn-secondary" type="button" data-dismiss="modal">Cancel</button>
                        <button class="btn btn-danger" v-on:click="deleteRecord">Delete</button>
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
import { router } from '../_helpers/router'

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
            submitted: false,
            idRecordToDelete: '',
            entryTypeIncomeId: ''
        }
    },
    computed: {
        ...mapState({
            alert: state => state.alert,
            userInfo: state => state.userInfo
        })
    },
    methods: {        
        getAllRecordsForUser() {
            recordService.getByIdUser(this.userInfo.user.id).then(response => {
                this.records = response;
            });
        },
        addNewRecord() {
            this.getAccountId();
            this.getCategoryId();
            var finished = recordService.addNew(this.newRecord);
            return finished;
        },
        updateRecord() {
            this.getAccountId();
            this.getCategoryId();
            var finished = recordService.update(this.newRecord);
            return finished;
        },
        async deleteRecord () {
            await recordService.delete(this.idRecordToDelete);
            location.reload()
        },
        handleSubmitRecord(e) {
            this.submitted = true;
            this.$validator.validate().then(async (valid) => {
                if (valid) {
                    e.preventDefault();
                    if(this.checkIfRecordExists() == true) {
                        await this.updateRecord();
                    }
                    else {
                        await this.addNewRecord();
                    }
                    location.reload();
                }
                else {
                    e.preventDefault();
                    return;
                }
            });
        },
        getCategories() {
            for(var i=0; i<this.entryTypes.length; i++) {
                if(this.entryTypes[i].name == this.selectedEntryType) {
                    this.categories = this.entryTypes[i].categories;
                    return;
                }
            }
            this.categories = null;
            return;
        },
        
        getEntryTypesWithCategories() {            
            var finished = entryTypeService.getWithCategories().then(response => {            
                this.entryTypes = response;
            });
            return finished;
        },
        getAllAccountsForUser() {
            var finished = accountService.getByIdUser(this.userInfo.user.id).then(response => {            
                this.accounts = response;
            });
            return finished;
        },
        getAccountId(){
            for(var i=0; i<this.accounts.length; i++) {
                if(this.accounts[i].name == this.selectedAccount) {
                    this.newRecord.accountId = this.accounts[i].id;
                    return;
                }
            }
            this.newRecord.accountId = null;
            return;
        },
        getCategoryId(){
            for(var i=0; i<this.categories.length; i++) {
                if(this.categories[i].name == this.selectedCategory) {
                    this.newRecord.categoryId = this.categories[i].id;
                    return;
                }
            }
            this.newRecord.accountId = null;
            return;
        },
        getCurrentRecord(index){
            this.newRecord = {...this.records[index]};
            this.selectedAccount = this.records[index].account.name;
            this.getSelectedEntryType(this.records[index].category);
            this.selectedCategory = this.records[index].category.name;
        },
        checkIfRecordExists() {
            if(this.records.length == 0) return false;
            for(var i=0; i < this.records.length; i++) {
                if(this.records[i].id == this.newRecord.id) {
                    return true;
                } 
            }
            return false;
        },
        checkIfAnyAccountsExist(){
            if(this.accounts.length == 0) router.push({ path: `/dashboard` })
        },
        getEntryTypeIncomeId () {            
            for(var i = 0; i < this.entryTypes.length; i++)
            {
                if(this.entryTypes[i].name == 'Income')
                    this.entryTypeIncomeId = this.entryTypes[i].id;
            }
        },
        checkIfCategoryIsIncome (category) {
            if (this.entryTypeIncomeId == category.entryTypeId) return true;
            else return false;
        },
        getSelectedEntryType(category) {
            for(var i = 0; i < this.entryTypes.length; i++)
            {
                if(this.entryTypes[i].id == category.entryTypeId) this.selectedEntryType = this.entryTypes[i].name; 
            }
        },
        clearNewRecordData() {
            this.newRecord.accountId = "";
            this.newRecord.categoryId = "";
            this.newRecord.description = "";
            this.newRecord.moneyAmount = "";
            delete this.newRecord.id;
            delete this.newRecord.createdTimestamp;
            delete this.newRecord.category;
            delete this.newRecord.account;
            delete this.newRecord.entryType;
            this.selectedAccount = "";
            this.selectedEntryType = "";
            this.selectedCategory = "";
            this.$validator.reset();
        }
    },
    created: async function(){
        this.getAllRecordsForUser();
        await this.getEntryTypesWithCategories();
        this.getEntryTypeIncomeId();
        await this.getAllAccountsForUser();                
        this.checkIfAnyAccountsExist();
    },
    watch: {
        selectedEntryType: function (newEntryType, oldEntryType) {
            this.getCategories();
            if(this.checkIfRecordExists() == false) this.selectedCategory = "";
        }
    }
};
</script>