import * as React from 'react';
import { connect, connectAdvanced } from 'react-redux';
//import { Link } from 'react-router-dom';
import { ApplicationState } from '../../store';
import * as EmployeesStore from '../../store/EmployeesPanel/EmployeesPanel';
import * as EmployeesListStore from '../../store/EmployeesPanel/EmployeesList';
import classnames from 'classnames';
import { Employee } from '../../api/Employee';

type Props =
    EmployeesStore.EmployeesPanelState &
    typeof EmployeesListStore.actionCreators &
    { id: number };

class EmployeeContainer extends React.PureComponent<Props> {
    render() {
        var employee: Employee = this.props.list.employees.find(
            (value, index) => this.props.list.employees.ids[index] == this.props.id
        );
        if (!employee) return null;
        let jobStartDateString: string;
        return (
            <li className={classnames(
                    'employeeContainer',
                    this.props.list.selectedEmployee == employee.id ? 'selected' : ''
                )}
                onClick={() => {
                    // If not selected
                    if (this.props.list.selectedEmployee != employee.id)
                        this.props.select(employee.id);
                    // If already selected
                    else this.props.select(null);
                }}
            >
                <div className="employeeContainer__id" title={"ID: " + employee.id}>#{employee.id}</div>
                <div className="employeeContainer__name" title={"Name: " + employee.name}>{employee.name}</div>
                <div className="employeeContainer__surname" title={"Surname: " + employee.surname}>{employee.surname}</div>
                { employee.patronymic && (
                    <div className="employeeContainer__patronymic" title={"Patronymic: " + employee.patronymic}>{employee.patronymic}</div>
                ) }
                <div className="employeeContainer__department" title={"Department: " + employee.department}>{employee.department}</div>
                <div className="employeeContainer__position" title={"Position: " + employee.position}>{employee.position}</div>
                { employee.supervisor && (
                    <div className="employeeContainer__supervisor" title={"Supervisor ID: " + employee.supervisor}>#{employee.supervisor}</div>
                ) }
                { (jobStartDateString = employee.jobStartDate.toLocaleString('en-GB', {
                    weekday: 'long',
                    month: 'long',
                    day: 'numeric',
                    year: 'numeric',
                    hour: 'numeric',
                    minute: 'numeric',
                    second: 'numeric'
                })) && false }
                <div className="employeeContainer__jobStartDate" title={"Job start date: " + jobStartDateString}>{jobStartDateString}</div>
            </li>
        )
    }
}

export default connect(
    (state: ApplicationState) => state.employeesPanel as EmployeesStore.EmployeesPanelState,
    EmployeesListStore.actionCreators
)(EmployeeContainer as any);