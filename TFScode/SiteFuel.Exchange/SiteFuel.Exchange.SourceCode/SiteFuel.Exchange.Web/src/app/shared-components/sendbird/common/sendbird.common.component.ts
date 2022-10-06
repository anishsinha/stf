export class SendBirdCommonComponent {
    
    public static getLastSeen(startDateTime: Date) {
        let results;
        try {
            let toTime = new Date().getTime();
            let fromTime = startDateTime.getTime();
            let datediff = toTime - fromTime;
            let seconds = Math.floor(datediff / 1000);
            let minutes = Math.floor(seconds / 60);
            let hours = Math.floor(minutes / 60);
            let days = Math.floor(hours / 24);
            let weeks = Math.floor(days / 7);
            let months = Math.floor(weeks / 4);
            let years = Math.floor(days / 365);
            let getSeconds = Math.abs(seconds);
            switch (true) {
                case (getSeconds == 0):
                    results = "just now";
                    break;
                case (getSeconds == 1):
                    results = this.getTense(seconds, 'second', true);
                    break;
                case (getSeconds > 1 && getSeconds < 60):
                    results = this.getTense(seconds, 'second', false);
                    break;
                case (getSeconds >= 60 && getSeconds < 120):
                    results = this.getTense(minutes, 'mintue', true);
                    break;
                case (getSeconds >= 120 && getSeconds < 3600):
                    results = this.getTense(minutes, 'mintue', false);
                    break;
                case (getSeconds >= 3600 && getSeconds < 7200):
                    results = this.getTense(hours, 'hour', true);
                    break;
                case (getSeconds >= 7200 && getSeconds < 86400):
                    results = this.getTense(hours, 'hour', false);
                    break;
                case (getSeconds >= 86400 && getSeconds < 172800):
                    results = this.getTense(days, 'day', true);
                    break;
                case (getSeconds >= 172800 && getSeconds < 604800):
                    results = this.getTense(days, 'day', false);
                    break;
                case (getSeconds >= 604800 && getSeconds < 1209600):
                    results = this.getTense(weeks, 'week', true);
                    break;
                case (getSeconds >= 1209600 && getSeconds < 2419200):
                    results = this.getTense(weeks, 'week', false);
                    break;
                case (getSeconds >= 2419200 && getSeconds < 4838400):
                    results = this.getTense(months, 'month', true);
                    break;
                case (getSeconds >= 4838400 && getSeconds < 31536000):
                    results = this.getTense(months, 'month', false);
                    break;
                case (getSeconds >= 31536000 && getSeconds < 63072000):
                    results = this.getTense(years, 'year', true);
                    break;
                case (getSeconds >= 63072000):
                    results = this.getTense(years, 'year', false);
                    break;
            }
        }
        catch (err) {
            results = "";
            console.log("getLastSeen", err);
        }
        return results;
    }
    private static getTense(time, tense, singular) {
        let getTime = Math.abs(time);
        if (singular == true) {
            if (time > 0) {
                if (tense == 'hour' || tense == 'year')
                    return 'An ' + tense + ' ago';
                else
                    return 'A ' + tense + ' ago';
            } else {
                if (tense == 'hour' || tense == 'year')
                    return 'After an ' + tense;
                else
                    return 'After a ' + tense;
            }

        } else {
            if (time > 0)
                return getTime + ' ' + tense + 's ago';
            else
                return 'After ' + getTime + ' ' + tense + 's';

        }
    }

}