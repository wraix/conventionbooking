import React from "react";

import logo from "../assets/beer.svg";

const Hero = () => (
  <div className="text-center hero my-5">
    <img className="mb-3 app-logo" src={logo} alt="Convention Booking logo" width="120" />
    <h1 className="mb-4">Convention Booking</h1>

    <p className="lead">
      Do you enjoy a cold beer?
      We provide conventions around the world with pasionate brew masters that love to educate and entertain while you try their goods.
      Signup for a convention and secure your seat with your favorite brew master today!
    </p>
  </div>
);

export default Hero;
