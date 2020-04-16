import * as React from 'react';
import EmployeeCreator from './EmployeeCreator';
import EmployeesButtons from './EmployeesButtons';
import EmployeesList from './EmployeesList';

export default class EmployeesPanel extends React.PureComponent {
    render() {
        return (
            <div className="employeesPanel">
                <EmployeeCreator/>
                <EmployeesButtons/>
                <EmployeesList/>
            </div>
        )
    }
}