﻿import Vue from 'vue';
import VueRouter from 'vue-router';

import LoginPage from '../login/LoginPage'
import RegisterPage from '../register/RegisterPage'
import DashboardPage from '../dashboard/DashboardPage'
import RecordsPage from '../records/RecordsPage'
import AnalyticsPage from '../analytics/AnalyticsPage'

Vue.use(VueRouter);

export const router = new VueRouter({
    mode: 'history',
    routes: [
        { path: '/login', component: LoginPage },
        { path: '/register', component: RegisterPage },
        { path: '/dashboard', component: DashboardPage },
        { path: '/records', component: RecordsPage },
        { path: '/analytics', component: AnalyticsPage},

        // otherwise redirect to home
        { path: '*', redirect: '/dashboard' }
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