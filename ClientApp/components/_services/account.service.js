import config from 'config';
import { authHeader, handleResponse, handleError } from '../_helpers';

export const accountService = {
    getByIdUser, 
    addNew,
    update,
    delete: _delete
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

function update(account) {
    const requestOptions = {
        method: 'PUT',
        headers: { ...authHeader(), 'Content-Type': 'application/json' },
        body: JSON.stringify(account)
    };

    return fetch(`${config.apiUrl}/accounts/${account.id}`, requestOptions).then(handleResponse).catch(handleError);
}

// prefixed function name with underscore because delete is a reserved word in javascript
function _delete(idAccount) {
    const requestOptions = {
        method: 'DELETE',
        headers: authHeader()
    };

    return fetch(`${config.apiUrl}/accounts/${idAccount}`, requestOptions).then(handleResponse);
}