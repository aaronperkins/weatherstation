import React, { Component } from 'react';
import { RadialGauge, LinearGauge } from 'react-canvas-gauges';
import { Container, Row, Col } from 'reactstrap';
import { format } from "date-fns";
import './Weather.css';

export class Weather extends Component {
  static displayName = Weather.name;

  constructor(props) {
    super(props);
    this.state = { weather: {}, loading: true };
  }

  componentDidMount() {
    this.populateWeatherData();
    this.interval = setInterval(() => this.populateWeatherData(), 1000);
  }

  componentWillUnmount() {
    clearInterval(this.interval);
  }

  static renderCurrentWeather(weather) {
    return (
      <Container>
        <Row><Col xs={2}><b>Date:</b></Col><Col>{format(new Date(weather.date), "MMMM dd, yyyy H:mm:ss")}</Col></Row>
        <Row><Col xs={2}><b>Temp. (°C):</b></Col><Col>{weather.temperatureC.toFixed(2)}</Col></Row>
        <Row><Col xs={2}><b>Temp. (°F):</b></Col><Col>{weather.temperatureF.toFixed(2)}</Col></Row>
        <Row><Col xs={2}><b>Pressure (hPa):</b></Col><Col>{weather.pressure.toFixed(2)}</Col></Row>

        <Row style={{paddingTop: 10}}>
          <Col>
            <RadialGauge
              height="300"
              width="300"
              units='hPa'
              title='Pressure'
              value={weather.pressure}
              minValue={0}
              maxValue={1500}
              majorTicks={['0', '100', '300', '500', '700', '900', '1100', '1300', '1500']}
              minorTicks={10}
            />
          </Col>
          <LinearGauge
              value={weather.temperatureC}
              width="120"
              height="400"
              units="°C"
              minValue={0}
              start-angle="90"
              ticks-angle="180"
              value-box="false"
              maxValue={220}
              major-ticks="0,20,40,60,80,100,120,140,160,180,200,220"
              minor-ticks="2"
              stroke-ticks="true"
              highlights='[ {"from": 100, "to": 220, "color": "rgba(200, 50, 50, .75)"} ]'
              color-plate="#fff"
              border-shadow-width="0"
              borders="false"
              needle-type="arrow"
              needle-width="2"
              needle-circle-size="7"
              needle-circle-outer="true"
              needle-circle-inner="false"
              animation-duration="1500"
              animation-rule="linear"
              bar-width="10"
            />
          <Col>
            <LinearGauge
              value={weather.temperatureF}
              width="120"
              height="400"
              units="°F"
              min-value="0"
              start-angle="90"
              ticks-angle="180"
              value-box="false"
              max-value="220"
              major-ticks="0,20,40,60,80,100,120,140,160,180,200,220"
              minor-ticks="2"
              stroke-ticks="true"
              highlights='[ {"from": 100, "to": 220, "color": "rgba(200, 50, 50, .75)"} ]'
              color-plate="#fff"
              border-shadow-width="0"
              borders="false"
              needle-type="arrow"
              needle-width="2"
              needle-circle-size="7"
              needle-circle-outer="true"
              needle-circle-inner="false"
              animation-duration="1500"
              animation-rule="linear"
              bar-width="10"
            />
          </Col>
        </Row>
      </Container>
    );
  }

  render() {
    let contents = this.state.loading
      ? <p><em>Loading...</em></p>
      : Weather.renderCurrentWeather(this.state.weather);

    return (
      <div>
        <h1 id="tabelLabel" >Current Weather</h1>
        {contents}
      </div>
    );
  }

  async populateWeatherData() {
    const response = await fetch('weather');
    const data = await response.json();
    this.setState({ weather: data, loading: false });
  }
}
