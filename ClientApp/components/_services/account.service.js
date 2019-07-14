import config from 'config';
import { authHeader, handleResponse, handleError } from '../_helpers';

export const accountService = {
    getByIdUser, 
    addNew
};

function getByIdUser(idUser) {
    const requestOptions = {
        method: 'GET',
        headers: authHeader()
    };

    return fetch(`${config.apiUrl}/accounts/${idUser}`, requestOptions).then(handleResponse);
}

function addNew(account) {
    const requestOptions = {
        method: 'POST',
        headers: { ...authHeader(), 'Content-Type': 'application/json'},
        body: JSON.stringify(account)
    };

    return fetch(`${config.apiUrl}/accounts/addNew`, requestOptions).then(handleResponse).catch(handleError);
}