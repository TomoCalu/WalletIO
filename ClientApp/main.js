import Vue from 'vue';
//import Vuetify from 'vuetify';
import VeeValidate from 'vee-validate';
import Chart from 'chart.js';
import babelPolyfill from 'babel-polyfill';

/*Fonts*/
import './vendor/fontawesome-free/css/all.min.css'
import './css/nonito-fonts.css'

/* Styles*/
import './css/sb-admin-2.min.css'
//import './vendor/datatables/dataTables.bootstrap4.css'
import './css/custom.css'

/*Bootstrap core JavaScript-*/
//import './vendor/jquery/jquery.min.js'
import './vendor/bootstrap/js/bootstrap.bundle.min.js'
//import './vendor/datatables/jquery.dataTables.min.js'
//import './vendor/datatables/dataTables.bootstrap4.min.js'

/*Core plugin JavaScript*/
//import './vendor/jquery-easing/jquery.easing.min.js'

/*Custom scripts for all pages*/
//import './js/sb-admin-2.min.js'

/*Page level plugins*/
//import './vendor/chart.js/Chart.min.js'

/*Page level custom scripts*/
//import './js/demo/chart-area-demo.js'
//import './js/demo/chart-pie-demo.js'

import App from './App';
import { router } from './components/_helpers';
import { store } from './components/_store';

//Vue.use(Vuetify)
Vue.use(VeeValidate);

/*import { configureFakeBackend } from './components/_helpers';
configureFakeBackend();*/

new Vue({
    el: '#app-root',
    store,
    router,
    render: h => h(App)
});