/**
 * Classes that are located here are definition of data
 * exchanged between the client and server
 */

import UniqueArray from "../utils/UniqueArray";
import { WithAllKeys } from "../utils/types";

export interface Employee {
    id: number;
    name: string;
    surname: string;
    patronymic?: string;
    department: string;
    position: string;
    supervisor?: number;
    jobStartDate: Date;
}

// Get
export interface GetRequest {
    page?: number;
}
export type GetResponse = UniqueArray<Employee>;

// Put
export interface PutRequest {
    name: string;
    surname: string;
    patronymic?: string;
    department: string;
    position: string;
    supervisor?: number;
}
export const putRequestValidation: { [key in keyof WithAllKeys<PutRequest>]: (value: any) => boolean } = {
    name: value => typeof(value) == 'string' && /^[a-zA-Z0-9-_]{1,50}$/.test(value),
    surname: value => typeof(value) == 'string' && /^[a-zA-Z0-9-_]{1,50}$/.test(value),
    patronymic: value => !value || typeof(value) == 'string' && /^[a-zA-Z0-9-_]{1,50}$/.test(value),
    department: value => typeof(value) == 'string' && /^[a-zA-Z0-9-_]{1,50}$/.test(value),
    position: value => typeof(value) == 'string' && /^[a-zA-Z0-9-_]{1,50}$/.test(value),
    supervisor: value => !value || typeof(value) == 'string' && /^[1-9][0-9]*$/.test(value) && parseInt(value) >= 0,
}
export interface PutResponse {
    result: number;
    validationErrors: { [key in keyof WithAllKeys<PutRequest>]: string; };
}

// Delete
export type DeleteRequest = number;
//export type DeleteResponse = void;