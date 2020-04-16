import { Action, Reducer } from "redux";
import { AppThunkAction } from "..";
import { PutRequest } from "../../api/Employee";
import { WithAllKeys, RequiredKeys, OptionalKeys, WithRequiredKeys, WithOptionalKeys } from "../../utils/types";

// Metadata
//const storeName = 'EmployeeCreator';

// State
export type EmployeeCreatorState = {
    fields: {
        [key in keyof WithRequiredKeys<PutRequest>]: {
            value: string;
            incorrect: boolean;
        }
    } & {
        [key in keyof WithOptionalKeys<PutRequest>]: {
            value: string | undefined;
            incorrect: boolean;
        }
    }
}
const employeeCreatorState: EmployeeCreatorState = {
    fields: {
        name: {
            value: "",
            incorrect: true,
        },
        surname: {
            value: "",
            incorrect: true,
        },
        patronymic: {
            value: "",
            incorrect: false,
        },
        department: {
            value: "",
            incorrect: true,
        },
        position: {
            value: "",
            incorrect: true,
        },
        supervisor: {
            value: "",
            incorrect: false,
        },
    }
}

// Actions
export interface UpdateAction {
    type: 'UPDATE',
    data: Partial<EmployeeCreatorState['fields']>
}

// Actions union type
export type KnownAction = 
    UpdateAction;

// Action creators
export const actionCreators = {
    update: (params: Partial<EmployeeCreatorState['fields']>): AppThunkAction<KnownAction> => async (dispatch, getState) => {
        dispatch({ type: 'UPDATE', data: params });
    },
}

export const reducer: Reducer<EmployeeCreatorState> = (state: EmployeeCreatorState | undefined, incomingAction: Action): EmployeeCreatorState => {
    // Initial state
    if (state === undefined) {
        return employeeCreatorState;
    }

    const action = incomingAction as KnownAction;
    switch (action.type) {
        case 'UPDATE':
            return { ...state,
                fields: { ...state.fields,
                    ...action.data
                }
            }
        default:
            return state;
    }
}