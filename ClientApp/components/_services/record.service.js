import config from 'config';
import { authHeader, handleResponse, handleError } from '../_helpers';

export const recordService = { 
    addNew,
    getByIdUser
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