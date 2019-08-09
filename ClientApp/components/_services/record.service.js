import config from 'config';
import { authHeader, handleResponse, handleError } from '../_helpers';

export const recordService = { 
    addNew,
    getByIdUser,
    update,
    delete: _delete,
    getSpendingsSums,
    getBalanceIncomeTrends,
    getBalanceSpendingsTrends
};

function addNew(record) {
    const requestOptions = {
        method: 'POST',
        headers: { ...authHeader(), 'Content-Type': 'application/json'},
        body: JSON.stringify(record)
    };

    return fetch(`${config.apiUrl}/records/addNew`, requestOptions).then(handleResponse).catch(handleError);
}

function getByIdUser(idUser) {
    const requestOptions = {
        method: 'GET',
        headers: authHeader()
    };

    return fetch(`${config.apiUrl}/records/${idUser}`, requestOptions).then(handleResponse);
}

function update(record) {
    const requestOptions = {
        method: 'PUT',
        headers: { ...authHeader(), 'Content-Type': 'application/json' },
        body: JSON.stringify(record)
    };

    return fetch(`${config.apiUrl}/records/${record.id}`, requestOptions).then(handleResponse).catch(handleError);
}

// prefixed function name with underscore because delete is a reserved word in javascript
function _delete(idRecord) {
    const requestOptions = {
        method: 'DELETE',
        headers: authHeader()
    };

    return fetch(`${config.apiUrl}/records/${idRecord}`, requestOptions).then(handleResponse);
}

function getSpendingsSums(idUser) {
    const requestOptions = {
        method: 'GET',
        headers: authHeader()
    };

    return fetch(`${config.apiUrl}/records/recordDataSum/${idUser}`, requestOptions).then(handleResponse);

}

function getBalanceIncomeTrends(idUser) {
    const requestOptions = {
        method: 'GET',
        headers: authHeader()
    };

    return fetch(`${config.apiUrl}/records/balanceIncomeTrends/${idUser}`, requestOptions).then(handleResponse);

}

function getBalanceSpendingsTrends(idUser) {
    const requestOptions = {
        method: 'GET',
        headers: authHeader()
    };

    return fetch(`${config.apiUrl}/records/balanceSpendingsTrends/${idUser}`, requestOptions).then(handleResponse);

}