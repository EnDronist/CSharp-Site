import * as React from 'react';
import { connect, ReactReduxContext } from 'react-redux';
import EmployeeContainer from './EmployeeContainer';
import { ApplicationState } from '../../store/.';
import * as EmployeesListStore from '../../store/EmployeesPanel/EmployeesList';
import EmployeeCreator from './EmployeeCreator';

type Props =
    EmployeesListStore.EmployeesListState &
    typeof EmployeesListStore.actionCreators;

class EmployeesList extends React.PureComponent<Props> {
    render() {
        return (
            <ul className="employeesPanel__list">
                { this.props.employees.map((_, index) => (
                    React.createElement(EmployeeContainer, {
                        key: this.props.employees.ids[index],
                        id: this.props.employees.ids[index],
                    } as any)
                    // <EmployeeContainer
                    //     id={this.props.employees.ids[index]}
                    //     key={this.props.employees.ids[index]}
                    // />
                )) }
            </ul>
        )
    }
}

export default connect(
    (state: ApplicationState) => state.employeesPanel.list,
    EmployeesListStore.actionCreators
)(EmployeesList as any);