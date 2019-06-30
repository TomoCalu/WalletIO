import Vue from 'vue';
//import Vuetify from 'vuetify';
import VeeValidate from 'vee-validate';
//import 'vuetify/dist/vuetify.min.css'

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