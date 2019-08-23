import Vue from 'vue';
import Vuex from 'vuex';

import { alert } from './alert.module';
import { userInfo } from './user.module';

Vue.use(Vuex);

export const store = new Vuex.Store({
    modules: {
        alert,
        userInfo
    }
});
