import {React} from "react";
import { Table, Button } from "reactstrap"

const TalksList = (props) => {
    return (
        <Table>
            <thead>
            <tr>
                <th>Talker</th>
                <th>Title</th>
                <th>Registered</th>
            </tr>
            </thead>
            <tbody>
                {props.data && props.data.map( row => (
                    <tr key={row.id ? row.id : ''}>
                        <td>{row.talker && row.talker.name ? row.talker.name : 'n/a'}</td>
                        <td>{row.title ? row.title : 'n/a'}</td>
                        <td>{!!row.attending ? "Attending" : <Button>Signup</Button>}</td>
                    </tr>
                ))}
            </tbody>
        </Table>
    );
}

export default TalksList;