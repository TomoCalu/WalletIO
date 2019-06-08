<template>
    <div>
        <nav class="navbar navbar-expand-lg navbar-dark bg-dark" v-if="account.status.loggedIn == true">
            <a class="navbar-brand" href="#">WalletIO</a>
            <button class="navbar-toggler" type="button" data-toggle="collapse" data-target="#navbarSupportedContent" aria-controls="navbarSupportedContent" aria-expanded="false" aria-label="Toggle navigation">
                <span class="navbar-toggler-icon"></span>
            </button>

            <div class="collapse navbar-collapse" id="navbarSupportedContent">
                <ul class="navbar-nav mr-auto">
                <li class="nav-item active">
                    <a class="nav-link" href="#">Home <span class="sr-only">(current)</span></a>
                </li>
                <li class="nav-item">
                    <router-link class="nav-link" to="/records">Records</router-link>
                </li>
                <li class="nav-item">
                    <router-link class="nav-link" to="/login">Log out</router-link>
                </li>
                </ul>
                <form class="form-inline my-2 my-lg-0">
                <input class="form-control mr-sm-2" type="search" placeholder="Search" aria-label="Search">
                <button class="btn btn-outline-success my-2 my-sm-0" type="submit">Search</button>
                </form>
            </div>
        </nav>
        <div class="jumbotron">
            <div class="container">
                <div class="row">
                    <div class="col-sm-6 offset-sm-3"> 
                        <div v-if="alert.message" :class="`alert ${alert.type}`">{{alert.message}}</div>
                        <router-view></router-view>
                    </div>
                </div>
            </div>
        </div>
    </div>    
</template>

<script>
import { mapState, mapActions } from 'vuex'

export default {
    name: 'app-root',
    computed: {
        ...mapState({
            alert: state => state.alert,
            account: state => state.account
        })
    },
    methods: {
        ...mapActions({
            clearAlert: 'alert/clear' 
        })
    },
    watch: {
        $route (to, from){
            // clear alert on location change
            this.clearAlert();
        }
    } 
}
</script>
