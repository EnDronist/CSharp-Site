import * as React from 'react';
import { connect } from 'react-redux';
import { ApplicationState } from '../../store/.';
import * as EmployeesListStore from '../../store/EmployeesPanel/EmployeesList';
import classnames from 'classnames';

type Props =
    EmployeesListStore.EmployeesListState &
    typeof EmployeesListStore.actionCreators;

class EmployeesButtons extends React.PureComponent<Props> {
    render() {
        return (
            <div className="employeesPanel__buttons">
                {/* PREV */}
                <div className={classnames(
                        'button', 'previous', this.props.isLoading ? 'disabled' : ''
                    )}
                    onClick={() => {
                        this.props.previous();
                        this.props.get({});
                }}>
                    <span>PREV</span>
                </div>
                {/* UPDATE */}
                <div className={classnames(
                        'button', 'get', this.props.isLoading ? 'disabled' : ''
                    )}
                    onClick={() => this.props.get({})}
                >
                    <span>UPDATE({this.props.page})</span>
                </div>
                {/* NEXT */}
                <div className={classnames(
                        'button', 'next', this.props.isLoading ? 'disabled' : ''
                    )}
                    onClick={() => {
                        this.props.next();
                        this.props.get({});
                }}>
                    <span>NEXT</span>
                </div>
                {/* PUT */}
                <div className={classnames(
                        'button', 'put', this.props.isLoading ? 'disabled' : ''
                    )}
                    onClick={() => this.props.put()}
                >
                    <span>PUT</span>
                </div>
                {/* DELETE */}
                <div className={classnames(
                        'button', 'delete', this.props.isLoading ? 'disabled' : ''
                    )}
                    onClick={() => {
                        if (this.props.selectedEmployee)
                            this.props.delete(this.props.selectedEmployee);
                    }}
                >
                    <span>DELETE{this.props.selectedEmployee ? `(${this.props.selectedEmployee})` : ''}</span>
                </div>
            </div>
        )
    }
}

export default connect(
    (state: ApplicationState) => state.employeesPanel.list as EmployeesListStore.EmployeesListState,
    EmployeesListStore.actionCreators
)(EmployeesButtons as any);