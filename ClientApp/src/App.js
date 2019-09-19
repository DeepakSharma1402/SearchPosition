import React, { Component } from 'react';


export default class App extends Component {
    static displayName = App.name;

    constructor(props) {
        super(props);

        this.state = {
            url: '',
            keyword: '',
            searchPositions: [],
            loading: true,
            viewForm: false
        };

        this.websiteUrl = "";
        this.webKeyword = "";

        this.fetchResults = null;

        this.fetchData = this.fetchData.bind(this);

        this.apiAddress = '';

        this.search = this.search.bind(this);
        this.updateUrl = this.updateUrl.bind(this);
        this.updateKeyword = this.updateKeyword.bind(this);
    }

    search() {
        this.fetchData();
        setTimeout(() => {
            this.setState({
                viewForm: true,
            });
        }, 300);
    }

    fetchData() {
        this.apiAddress = `api/SearchResults/GetPosition?url=${this.state.url}&keyword=${this.state.keyword}`;

	fetch(this.apiAddress, {
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json',
            }
        })
            .then(response => response.json())
            .then(data => {
                this.setState({ searchPositions: data });
            });

    }

    updateUrl(evt) {
        this.setState({ url: evt.target.value });
    }

    updateKeyword(evt) {
        this.setState({ keyword: evt.target.value });
    }

    renderSearchPositions() {
        let returnContent;

        if (this.state.searchPositions.length > 0) {
            returnContent = this.state.searchPositions.map((p) =>
                <span>
                   &nbsp;{p},
                </span>
            );
        }
        else {
            returnContent = 0;
        }

        return returnContent;
    }

    render() {

        let contents = "";

        if (this.state.viewForm) {
            contents = this.renderSearchPositions();            
        }

        return (
            <div>
                <div>
                    <span>
                        Enter Url :
                    </span>
                    <input type="text" onChange={this.updateUrl} />
                </div>
                <div>
                    <span>
                        Enter Keyword :
                    </span>
                    <input type="text" onChange={this.updateKeyword} />
                </div>
                <div>
                    <button id="btnSearch" onClick={this.search}>
                        Search
                    </button>
                </div>
                <div>
                    Index positions found on Google Search : {contents}
                </div>
            </div>
        );
    }
}
