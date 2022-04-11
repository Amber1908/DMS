import React from "react";
import { Route, Switch, useRouteMatch } from "react-router";
import Tab from "../Tab";
import ReportCreator from "./ReportCreator";
import ReportMainList from "./ReportMainList";
import ReportVersionHistory from "./ReportVersionHistory";

const ReportCreatorContainer = () => {
    const { path, url } = useRouteMatch();

    const setRefreshReport = () => {

    }

    return (
        <div className="section">
            <div className="container containerMax">
                <div className="row">
                    <div className="col-md-12">
                        <div className="ui-monoBlock ui-cardBlock ui-FX-shadow">
                            <div className={`ui-tableBlock`} style={{ width: "100%" }}>
                                <div className="ui-TabWrap">
                                    <Tab id="reportCreator" link={`${url}`} tabName="表單產生器" tabToolTip="表單產生器" />
                                </div>
                                <div id="mainContainer" className="ui-workspace">
                                    <Switch>
                                        <Route exact path={`${path}`} render={(routerProps) => <ReportMainList setRefreshReport={setRefreshReport} {...routerProps} />} />
                                        <Route exact path={`${path}/New`} render={(routerProps) => <ReportCreator setRefreshReport={setRefreshReport} {...routerProps} />} />
                                        <Route exact path={`${path}/:category`} render={(routerProps) => <ReportVersionHistory setRefreshReport={setRefreshReport} {...routerProps} />} />
                                        <Route exact path={`${path}/:category/New`} render={(routerProps) => <ReportCreator setRefreshReport={setRefreshReport} {...routerProps} />} />
                                        <Route exact path={`${path}/:category/New/:sourceReportID`} render={(routerProps) => <ReportCreator setRefreshReport={setRefreshReport} {...routerProps} />} />
                                        <Route exact path={`${path}/:category/:reportMID`} render={(routerProps) => <ReportCreator setRefreshReport={setRefreshReport} {...routerProps} />} />
                                    </Switch>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    );
};

export default ReportCreatorContainer;
