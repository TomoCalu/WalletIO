import config from 'config';
import { authHeader, handleResponse } from '../_helpers';

export const entryTypeService = {
    getWithCategories
};

function getWithCategories() {
    const requestOptions = {
        method: 'GET',
        headers: authHeader()
    };

    return fetch(`${config.apiUrl}/entryTypes`, requestOptions).then(handleResponse);
}