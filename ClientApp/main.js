import Vue from 'vue';
import VeeValidate from 'vee-validate';

import App from './App';
import { router } from './components/_helpers';
import { store } from './components/_store';

Vue.use(VeeValidate);

/*import { configureFakeBackend } from './components/_helpers';
configureFakeBackend();*/

new Vue({
    el: '#app-root',
    store,
    router,
    render: h => h(App)
});