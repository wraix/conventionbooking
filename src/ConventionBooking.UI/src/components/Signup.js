import {React} from "react";
import { Form, FormGroup, Button,Card, CardBody, CardTitle, CardText, Badge } from "reactstrap"

const Signup = (props) => {
    return (
        <Form>
            <FormGroup>
                <Card>
                <CardBody>
                    <div className="small" style={{color: 'grey', float:'right'}}><span>convention: {props.id}</span></div>
                    <CardTitle tag="h5">
                      {props.name}
                    </CardTitle>
                    <CardText>
                    This convention is taking place @ {props.venue}
                    </CardText>
                </CardBody>
                </Card>
            </FormGroup>

            <Button color="primary" onClick={props.signupHandler}>Signup</Button>
        </Form>
    );
}

export default Signup;