import { Action, Reducer } from 'redux';
import { Employee,
    GetRequest, GetResponse,
    PutRequest, PutResponse,
    DeleteRequest
} from '../../api/Employee';
import { AppThunkAction } from '../.';
import { EmployeeCreatorState } from './EmployeeCreator';
import UniqueArray from '../../utils/UniqueArray';
import { ValueOf, ValuesOf } from '../../utils/types';
//import sleep from '../utils/sleep';

// Metadata
const storeName = 'EmployeesList';

// State
export interface EmployeesListState {
    page: number;
    employees: UniqueArray<Employee>;
    selectedEmployee: number | null;
    isLoading: boolean;
}
const employeesListState: EmployeesListState = {
    page: 0,
    employees: new UniqueArray(),
    selectedEmployee: null,
    isLoading: false,
}

// Actions
export interface PageIncreaseAction { type: 'PAGE_INCREASE'; }
export interface PageDecreaseAction { type: 'PAGE_DECREASE'; }
export interface SelectEmployeeAction {
    type: 'SELECT_EMPLOYEE';
    data: number | null;
}
// Actions with server
export interface GetRequestAction { type: 'GET_REQUEST'; }
export interface GetResponseAction {
    type: 'GET_RESPONCE';
    data: GetResponse;
}
export interface PutRequestAction { type: 'PUT_REQUEST'; }
export interface PutResponseAction {
    type: 'PUT_RESPONCE';
    data: Employee | undefined;
}
export interface DeleteRequestAction { type: 'DELETE_REQUEST'; }
export interface DeleteResponseAction {
    type: 'DELETE_RESPONCE';
    data: number | null;
}

// Actions union type
export type KnownAction = 
    PageIncreaseAction |
    PageDecreaseAction |
    SelectEmployeeAction |
    GetRequestAction |
    GetResponseAction |
    PutRequestAction |
    PutResponseAction |
    DeleteRequestAction |
    DeleteResponseAction;

// Action creators
// export type ActionCreators = () => {
//     [key1 in keyof typeof actionCreators]: (params: {
//         [key2 in ValuesOf<Parameters<typeof actionCreators[key1]>>]:
//             ValuesOf<Parameters<typeof actionCreators[key1]>>[key2]
//     }) => void
// }

export const actionCreators = {
    next: (): AppThunkAction<KnownAction> => async (dispatch, getState) => {
        dispatch({ type: 'PAGE_INCREASE' });
    },
    previous: (): AppThunkAction<KnownAction> => async (dispatch, getState) => {
        dispatch({ type: 'PAGE_DECREASE' });
    },
    select: (id: number | null): AppThunkAction<KnownAction> => async (dispatch, getState) => {
        dispatch({ type: 'SELECT_EMPLOYEE', data: id });
    },
    get: (params: GetRequest): AppThunkAction<KnownAction> => async (dispatch, getState) => {
        // Checking if already loading
        const state = getState();
        const list = state.employeesPanel.list;
        if (list.isLoading) return;
        // Setting temporary state
        dispatch({ type: 'GET_REQUEST' });
        // Fake sleep
        //await sleep(500);
        // Getting data from server
        let result = await fetch(`api/Employees?page=${
            params.page ? params.page : list.page
        }`);
        try {
            var data = new UniqueArray(await result.json() as Array<Employee>);
            // Parsing Date type
            data.map((value: Employee) => value.jobStartDate = new Date(value.jobStartDate));
        }
        catch {
            console.log(`${storeName}.GetRequest: incorrect server response`);
            dispatch({ type: 'GET_RESPONCE', data: list.employees });
            return;
        }
        // Saving result
        dispatch({ type: 'GET_RESPONCE', data: data })
    },
    put: (): AppThunkAction<KnownAction> => async (dispatch, getState) => {
        // Check if already loading
        const state = getState();
        const list = state.employeesPanel.list;
        const fields = state.employeesPanel.creator.fields;
        if (list.isLoading) return;
        // Checking data before sending
        let correctForm = true;
        for (var key in fields) {
            var field = key as keyof typeof fields;
            if (fields[field].incorrect) {
                correctForm = false; break;
            }
        }
        if (!correctForm) return;
        // Collecting data to send
        let sendingDataRaw: Array<[string, any]> = Object.keys(fields).map(value => {
            var key = value as keyof EmployeeCreatorState['fields'];
            var field = fields[key];
            return [key, field ? field.value : undefined];
        })
        let sendingData: { [key in keyof EmployeeCreatorState['fields']]: string } = Object.assign({},
            ...sendingDataRaw.map(([k, v]) => ({[k]: v}))
        );
        console.log('sendingData');
        console.log(sendingData);
        // Correction data for send
        const fieldCorrection: { [key in keyof EmployeeCreatorState['fields']]: (value: string) => PutRequest[key] } = {
            name: value => value,
            surname: value => value,
            patronymic: value => value !== '' ? value : undefined,
            department: value => value,
            position: value => value,
            supervisor: value => value !== '' ? parseInt(value) : undefined,
        }
        let correctedDataRaw: Array<[string, any]> = Object.keys(sendingData).map(value => {
            var key = value as keyof EmployeeCreatorState['fields'];
            console.log(key);
            return [key, fieldCorrection[key](sendingData[key])];
        });
        let correctedData: PutRequest = Object.assign({},
            ...correctedDataRaw.map(([k, v]) => ({[k]: v}))
        );
        console.log('correctedData');
        console.log(correctedData);
        // Temporary state
        dispatch({ type: 'PUT_REQUEST' });
        // Sending data to server
        console.log(JSON.stringify(correctedData));
        let result = await fetch(`api/Employees`, {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(correctedData),
        });
        try {
            var responce = await result.json() as PutResponse;
        }
        catch {
            console.log(`${storeName}.GetRequest: incorrect server response`);
            return;
        }
        console.log(responce);
        //let data: PutResponse = {
        //    result: 20,
        //}
        // Checking server's response
        if (responce.validationErrors) {
            for (var key in responce.validationErrors) {
                console.log(responce.validationErrors[key as keyof typeof responce.validationErrors]);
            }
            dispatch({
                type: 'PUT_RESPONCE',
                data: undefined,
            });
            return;
        }
        // Saving result
        dispatch({
            type: 'PUT_RESPONCE',
            data: responce.result != 1 ? {
                id: responce.result,
                jobStartDate: new Date(Date.now()),
                ...correctedData
            } : undefined 
        });
    },
    delete: (params: DeleteRequest): AppThunkAction<KnownAction> => async (dispatch, getState) => {
        // Check if already loading
        const state = getState();
        const list = state.employeesPanel.list;
        if (list.isLoading) return;
        // Temporary state
        dispatch({ type: 'DELETE_REQUEST' });
        // Getting data from server
        let result = await fetch(`api/Employees?id=${params}`, { method: 'DELETE' });
        if (!result.ok) {
            if (result.status == 404) console.log(`${storeName}.GetRequest: employee not exists`);
            else console.log(`${storeName}.GetRequest: incorrect server response`);
            dispatch({ type: 'DELETE_RESPONCE', data: null });
            return;
        }
        // Saving result
        dispatch({ type: 'DELETE_RESPONCE', data: params });
    },
};

// ----------------
// REDUCER - For a given state and action, returns the new state. To support time travel, this must not mutate the old state.

export const reducer: Reducer<EmployeesListState> = (state: EmployeesListState | undefined, incomingAction: Action): EmployeesListState => {
    // Initial state
    if (state === undefined) {
        return employeesListState;
    }

    const action = incomingAction as KnownAction;
    switch (action.type) {
        case 'PAGE_INCREASE':
            return { ...state,
                page: state.page + 1,
            }
        case 'PAGE_DECREASE':
            return { ...state,
                page: state.page === 0 ? 0 : state.page - 1,
            }
        case 'SELECT_EMPLOYEE':
            return { ...state,
                selectedEmployee: action.data,
            }
        case 'GET_REQUEST':
            return { ...state,
                isLoading: true,
            }
        case 'GET_RESPONCE':
            return { ...state,
                employees: action.data,
                isLoading: false,
            }
        case 'PUT_REQUEST':
            return { ...state,
                isLoading: true,
            }
        case 'PUT_RESPONCE': {
            let newEmployees;
            if (action.data) {
                newEmployees = new UniqueArray(state.employees);
                newEmployees.push(action.data);
            }
            return { ...state,
                employees: newEmployees || state.employees,
                isLoading: false,
            }
        }
        case 'DELETE_REQUEST':
            return { ...state,
                isLoading: true,
            }
        case 'DELETE_RESPONCE': {
            //let newEmployees = state.employees.filter(value => value.id !== action.data);
            let newEmployees: UniqueArray<Employee>;
            var returnFunc = () => ({ ...state,
                employees: newEmployees || state.employees,
                isLoading: false,
            });
            if (!action.data) return returnFunc();
            let indexToDelete = state.employees.findIndex(value => value.id == action.data);
            if (indexToDelete == -1) return returnFunc();
            newEmployees = new UniqueArray(state.employees);
            if (indexToDelete >= 0) newEmployees.remove(indexToDelete);
            return returnFunc();
        }
        default:
            return state;
    }
};