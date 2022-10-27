import {React} from "react";
import { Form, FormGroup, Button,Card, CardBody, CardTitle, CardText, Badge } from "reactstrap"

const ReserveSeat = (props) => {
    return (
        <Form>
            <FormGroup>
                <Card>
                <CardBody>
                    <div className="small" style={{color: 'grey', float:'right'}}><span>talk: {props.id}</span></div>
                    <CardTitle tag="h5">
                      {props.title}
                    </CardTitle>
                    <CardText>
                    {props.description}
                    </CardText>
                    <Badge pill color="warning">
                        Number of seats: {props.remaningSeats}
                    </Badge>
                </CardBody>
                </Card>
            </FormGroup>

            <Button color="primary" onClick={props.signupHandler}>Signup</Button>
        </Form>
    );
}

export default ReserveSeat;