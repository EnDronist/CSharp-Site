import { combineReducers, Reducer } from 'redux';
import { EmployeesListState, reducer as EmployeesListReducer } from './EmployeesList';
import { EmployeeCreatorState, reducer as EmployeeCreatorReducer } from './EmployeeCreator';

export interface EmployeesPanelState {
    list: EmployeesListState;
    creator: EmployeeCreatorState;
}

export const reducer = combineReducers({
    list: EmployeesListReducer,
    creator: EmployeeCreatorReducer,
}) as Reducer<EmployeesPanelState>;