import config from 'config';
import { authHeader, handleResponse, handleError } from '../_helpers';

export const recordService = { 
    addNew
};

function addNew(record) {
    const requestOptions = {
        method: 'POST',
        headers: { ...authHeader(), 'Content-Type': 'application/json'},
        body: JSON.stringify(record)
    };

    return fetch(`${config.apiUrl}/records/addNew`, requestOptions).then(handleResponse).catch(handleError);
}