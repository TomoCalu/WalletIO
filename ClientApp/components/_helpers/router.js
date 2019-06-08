import Vue from 'vue';
import VueRouter from 'vue-router';

import HomePage from '../home/HomePage'
import LoginPage from '../login/LoginPage'
import RegisterPage from '../register/RegisterPage'
import RecordsPage from '../records/RecordsPage'

Vue.use(VueRouter);

export const router = new VueRouter({
    mode: 'history',
    routes: [
        { path: '/', component: HomePage },
        { path: '/login', component: LoginPage },
        { path: '/register', component: RegisterPage },
        { path: '/records', component: RecordsPage },

        // otherwise redirect to home
        { path: '*', redirect: '/' }
    ]
});


router.beforeEach((to, from, next) => {
    // redirect to login page if not logged in and trying to access a restricted page
    const publicPages = ['/login', '/register'];
    const authRequired = !publicPages.includes(to.path);
    const loggedIn = localStorage.getItem('user');

    if (authRequired && !loggedIn) {
        return next('/login');
    }

    next();
});