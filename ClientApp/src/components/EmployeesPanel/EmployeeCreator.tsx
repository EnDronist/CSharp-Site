import * as React from 'react';
import { connect } from 'react-redux';
import { ApplicationState } from '../../store/.';
import * as EmployeeCreatorStore from '../../store/EmployeesPanel/EmployeeCreator';
import { PutRequest, putRequestValidation } from '../../api/Employee';
import { WithAllKeys } from '../../utils/types';
import classnames from 'classnames';

type Props =
    EmployeeCreatorStore.EmployeeCreatorState &
    typeof EmployeeCreatorStore.actionCreators;

class EmployeeCreator extends React.PureComponent<Props> {
    onChange = (event: React.ChangeEvent<HTMLInputElement>) => {
        var name = event.target.name as keyof WithAllKeys<PutRequest>;
        console.log(name);
        if (!putRequestValidation[name](event.target.value)) {
            this.props.update({ [name]: { value: event.target.value, incorrect: true } });
        }
        else {
            this.props.update({ [name]: { value: event.target.value, incorrect: false } });
        }
    }

    render() {
        return (
            <form className="employeeCreator">
                <input className={classnames(
                    'employeeCreate__name',
                    this.props.fields.name.incorrect ? 'incorrect' : ''
                )} type="text" name="name" placeholder="Name"
                onChange={this.onChange}
                />
                <input className={classnames(
                    'employeeCreate__surname',
                    this.props.fields.surname.incorrect ? 'incorrect' : ''
                )} type="text" name="surname" placeholder="Surname"
                onChange={this.onChange}
                />
                <input className={classnames(
                    'employeeCreate__patronymic',
                    this.props.fields.patronymic.incorrect ? 'incorrect' : ''
                )} type="text" name="patronymic" placeholder="Patronymic"
                onChange={this.onChange}
                />
                <input className={classnames(
                    'employeeCreate__department',
                    this.props.fields.department.incorrect ? 'incorrect' : ''
                )} type="text" name="department" placeholder="Department"
                onChange={this.onChange}
                />
                <input className={classnames(
                    'employeeCreate__position',
                    this.props.fields.position.incorrect ? 'incorrect' : ''
                )} type="text" name="position" placeholder="Position"
                onChange={this.onChange}
                />
                <input className={classnames(
                    'employeeCreate__supervisor',
                    this.props.fields.supervisor.incorrect ? 'incorrect' : ''
                )} type="text" name="supervisor" placeholder="Supervisor ID"
                onChange={this.onChange}
                />
            </form>
        )
    }
}

export default connect(
    (state: ApplicationState) => state.employeesPanel.creator as EmployeeCreatorStore.EmployeeCreatorState,
    EmployeeCreatorStore.actionCreators
)(EmployeeCreator as any);