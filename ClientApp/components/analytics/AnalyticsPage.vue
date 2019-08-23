<template>
    <!-- Begin Page Content -->
    <div class="container-fluid bg-white"  v-if="accounts.length > 0" style="min-height:100vh">

        <div class="d-sm-flex align-items-center justify-content-between mb-4">
            <h1 class="h3 mb-0 text-gray-800">Analytics - Total sum: {{totalSum}} kn</h1>
        </div>

        <!-- Analytics income table-->
        <table class="table table-borderless table-condensed table-hover">
            <thead class="font-weight-bold text-dark bg-light">
                <tr>
                    <th style="width: 90%">Total income</th>
                    <th style="width: 10%">Amount</th>
                </tr>
            </thead>
            <tbody>
                <template>
                    <tr @click="toggle(incomeEntryType.id)" class="font-weight-bold hover-cursor" :class="{ opened: opened.includes(incomeEntryType.id) }" v-bind:key="incomeEntryType.id">
                        <td>{{ incomeEntryType.name }}</td>
                        <td>{{ incomeEntryType.moneyAmount }} <template v-if="incomeEntryType.moneyAmount != null">kn</template></td>
                    </tr>
                    <template v-if="opened.includes(incomeEntryType.id)">
                        <tr v-for="category in incomeEntryType.categories" class="hover-cursor" v-bind:key="category.id" v-on:click="includeCategoryId(category.id)">
                            <td class="pl-5" v-bind:class="[checkIfSelected(category.id)? '' : 'crossed-out-text font-italic text-gray-300']">{{ category.name }}</td>
                            <td>{{category.moneyAmount}} <template v-if="checkIfSelected(category.id)">kn</template> </td>
                        </tr>
                    </template>
                </template>
            </tbody>
        </table>

        <!-- Analytics spendings table-->
        <table class="table table-borderless table-condensed table-hover">
            <thead class="font-weight-bold text-dark bg-light">
                <tr>
                    <th style="width: 90%">Total expenses</th>
                    <th style="width: 10%">Amount</th>
                </tr>
            </thead>
            <tbody>
                <template v-for="entryType in entryTypesWithoutIncome">
                    <tr @click="toggle(entryType.id)" class="font-weight-bold hover-cursor" :class="{ opened: opened.includes(entryType.id) }" v-bind:key="entryType.id">
                        <td>{{ entryType.name }}</td>
                        <td>{{ entryType.moneyAmount }} <template v-if="entryType.moneyAmount != null">kn</template></td>
                    </tr>
                    <template v-if="opened.includes(entryType.id)">
                        <tr v-for="category in entryType.categories" class="hover-cursor" v-bind:key="category.id" v-on:click="includeCategoryId(category.id)">
                            <td class="pl-5" v-bind:class="[checkIfSelected(category.id)? '' : 'crossed-out-text font-italic text-gray-300']">{{ category.name }}</td>
                            <td>{{category.moneyAmount}} <template v-if="checkIfSelected(category.id)">kn</template></td>
                        </tr>
                    </template>
                </template>
            </tbody>
        </table>
        
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
            accounts: [],
            entryTypes: [],
            entryTypesWithoutIncome: [],
            incomeEntryType: {},
            opened: [],
            selectedCategories: [],
            selectedCategoriesString:'',
            dataPerCategory:'',
            totalSum: 0,
        }
    },
    computed: {
        ...mapState({
            alert: state => state.alert,
            userInfo: state => state.userInfo
        })
    },
    methods: {
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
        checkIfAnyAccountsExist(){
            if(this.accounts.length == 0) router.push({ path: `/dashboard` })
        },
        getEntryTypesWithoutIncome() {
            this.entryTypesWithoutIncome = [...this.entryTypes];
            for(var i =0; i < this.entryTypesWithoutIncome.length; i++)
            {
                if(this.entryTypesWithoutIncome[i].name == 'Income') {
                    this.entryTypesWithoutIncome.splice(i,1)
                }
            }
        },
        async getSpendingsPerCategory() {
            if (this.selectedCategories.length > 0) {
                    this.selectedCategories.forEach(selectedCategory => {
                    this.selectedCategoriesString = this.selectedCategoriesString.concat('selectedCategories=',selectedCategory,'&');
                });
                this.selectedCategoriesString  = this.selectedCategoriesString.substring(0, this.selectedCategoriesString.length - 1);
                await recordService.getSpendingAndIncomePerCategory(this.userInfo.user.id, this.selectedCategoriesString).then(response => {
                    this.dataPerCategory = response;
                });
                this.selectedCategoriesString='';
            }
            else {        
                await recordService.getSpendingAndIncomePerCategory(this.userInfo.user.id, null).then(response => {
                    this.dataPerCategory = response;
                });
            }

            this.getMoneyAmountPerCategory();
            this.getMoneyAmountPerEntryType();
            this.getEntryTypesWithoutIncome();
            this.getIncomeEntryType();
            this.getMoneyAmountSumTotal();

        },
        getIncomeEntryType() {
            for(var i =0; i < this.entryTypes.length; i++)
            {
                if(this.entryTypes[i].name == 'Income') {
                    this.incomeEntryType = {...this.entryTypes[i]};
                }
            }
        },
        toggle(id) {
            const index = this.opened.indexOf(id);
            if (index > -1) {
                this.opened.splice(index, 1)
            } else {
                this.opened.push(id)
            }
        },
        includeCategoryId(categoryId) {
            for(var i=0; i<this.selectedCategories.length; i++) {
                if(this.selectedCategories[i] == categoryId) {
                    this.selectedCategories.splice(i,1);                    
                    this.getSpendingsPerCategory();
                    return;
                }
            }
            this.selectedCategories.push(categoryId);
            this.getSpendingsPerCategory();
        },
        checkIfSelected(categoryId){
            for(var i=0; i<this.selectedCategories.length; i++){
                if(this.selectedCategories[i] == categoryId) return true;
            }
            return false;
        },
        getMoneyAmountPerCategory() {
            for(var i=0; i<this.entryTypes.length; i++){
                for(var j=0; j<this.entryTypes[i].categories.length; j++){                
                    this.$delete(this.entryTypes[i].categories[j], 'moneyAmount')
                    for(var k=0; k<this.dataPerCategory.key.length; k++){
                        if(this.entryTypes[i].categories[j].id == this.dataPerCategory.key[k]) {
                            this.$set(this.entryTypes[i].categories[j], 'moneyAmount', this.dataPerCategory.value[k]);
                        }
                    }
                }
            }
        },
        getMoneyAmountPerEntryType() {
            for(var i=0; i<this.entryTypes.length; i++){
                var sum = 0;
                for(var j=0; j<this.entryTypes[i].categories.length; j++){
                    if(this.entryTypes[i].categories[j].moneyAmount) {
                        sum = sum + this.entryTypes[i].categories[j].moneyAmount;
                    }
                }                
                this.$set(this.entryTypes[i], 'moneyAmount', sum);
            }      
        },
        getMoneyAmountSumTotal() {
            this.totalSum = 0;
            for(var i=0; i<this.entryTypes.length; i++){
                if(this.entryTypes[i].name == 'Income') this.totalSum = this.totalSum + this.entryTypes[i].moneyAmount;
                else this.totalSum = this.totalSum - this.entryTypes[i].moneyAmount;
            }
        }
    },
    created: async function(){
        await this.getEntryTypesWithCategories();
        await this.getAllAccountsForUser();                
        this.checkIfAnyAccountsExist();
        this.getEntryTypesWithoutIncome();
        this.getIncomeEntryType();
    }
};
</script>