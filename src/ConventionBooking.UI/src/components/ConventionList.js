import {React} from "react";
import { Table, Button } from "reactstrap"

const ConventionList = (props) => {
    return (
        <Table>
            <thead>
            <tr>
                <th>Name</th>
                <th>Venue</th>
                <th>Registered</th>
            </tr>
            </thead>
            <tbody>
                {props.data && props.data.map( row => (
                    <tr key={row.id ? row.id : ''}>
                        <td>{row.name && row.name ? row.name : 'n/a'}</td>
                        <td>{row.venue && row.venue.name ? row.venue.name : 'n/a'}</td>
                        <td>{!!row.attending ? "Attending" : <Button>Signup</Button>}</td>
                    </tr>
                ))}
            </tbody>
        </Table>
    );
}

export default ConventionList;