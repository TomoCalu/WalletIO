import Vue from 'vue';
import VeeValidate from 'vee-validate';
import Chart from 'chart.js';
import babelPolyfill from 'babel-polyfill';

/*Fonts*/
import './vendor/fontawesome-free/css/all.min.css'
import './css/nonito-fonts.css'

/* Styles*/
import './css/sb-admin-2.min.css'
import './css/custom.css'
import './css/simple-sidebar.css'

/*Bootstrap core JavaScript-*/
import './vendor/bootstrap/js/bootstrap.bundle.min.js'

import App from './App';
import { router } from './components/_helpers';
import { store } from './components/_store';

Vue.config.devtools = true;
Vue.use(VeeValidate);

new Vue({
    el: '#app-root',
    store,
    router,
    render: h => h(App)
});