<script setup>
import axios from "axios";
</script>

<template>
    <div class="calendarView">
        <h1>Auftragskalender</h1>
        <div class="calendarHeader">
            <div class="headerArrow" v-on:click="changeMonth(-1)">
                <!-- https://www.svgrepo.com -->
                <svg fill="#000000" width="32px" height="32px" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg"><g id="SVGRepo_bgCarrier" stroke-width="0"></g><g id="SVGRepo_tracerCarrier" stroke-linecap="round" stroke-linejoin="round"></g><g id="SVGRepo_iconCarrier"><path d="m4.431 12.822 13 9A1 1 0 0 0 19 21V3a1 1 0 0 0-1.569-.823l-13 9a1.003 1.003 0 0 0 0 1.645z"></path></g></svg>
            </div>
            <h3 class="headerMonth">{{ months[currentMonth] }} {{ currentYear }}</h3>
            <div class="headerArrow" v-on:click="changeMonth(1)">
                <svg fill="#000000" width="32px" height="32px" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg"><g id="SVGRepo_bgCarrier" stroke-width="0"></g><g id="SVGRepo_tracerCarrier" stroke-linecap="round" stroke-linejoin="round"></g><g id="SVGRepo_iconCarrier"><path d="M5.536 21.886a1.004 1.004 0 0 0 1.033-.064l13-9a1 1 0 0 0 0-1.644l-13-9A1 1 0 0 0 5 3v18a1 1 0 0 0 .536.886z"></path></g></svg>
            </div>
        </div>
        <div class="kalender">
            <div class="headerCell">Montag</div>
            <div class="headerCell">Dienstag</div>
            <div class="headerCell">Mittwoch</div>
            <div class="headerCell">Donnerstag</div>
            <div class="headerCell">Freitag</div>
            <div class="headerCell">Samstag</div>
            <div class="headerCell">Sonntag</div>
            <div v-for="d in calendarDays" v-bind:key="d.jsTimestamp" v-bind:class="{
                dayCell: true,
                disabled: d.month != currentMonth,
                weekend: !d.isWorkingDayMoFr,
                holiday: d.isPublicHoliday,
                today: d.jsTimestamp == today,

            }">

                <div class="dayHeader">
                    <div class="dayNumber">
                        {{ d.day }}<span v-if="d.month != currentMonth">.{{ d.month }}.</span>

                    </div>
                    <div class="holidayName">{{ d.schoolHolidayName }}</div>

                </div>
                <div class="dayData">
                    <template v-if="d.jobs.length">
                        {{ d.jobs }}
                    </template>
                </div>
            </div>
        </div>
    </div>
</template>

<script>
export default {
    data() {
        return {
            months: ['', 'Jänner', 'Februar', 'März', 'April', 'Mai', 'Juni', 'Juli', 'August', 'September', 'Oktober', 'November', 'Dezember'],
            currentYear: 2023,
            currentMonth: 12,
            today: Math.floor(Date.now() / 86_400_000) * 86_400_000,
            calendarDays: []
        };
    },
    async mounted() {
        const date = new Date();
        this.currentYear = date.getFullYear();
        this.currentMonth = date.getMonth() + 1;
        await this.loadCalendar();
    },
    methods: {
        async loadCalendar() {
            this.popup = {};
            //const guid = this.$route.params.guid;  // User GUID for requests
            try {
                const res = await axios.get(`calendar/${this.currentYear}/${this.currentMonth}`);
                this.calendarDays = res.data;
            } catch (e) {
                alert(JSON.stringify(e));
            }
        },
        async changeMonth(step) {
            const newMonth = Math.max(2001 * 12, Math.min(2100 * 12, this.currentYear * 12 + (this.currentMonth - 1) + step));
            this.currentYear = Math.floor(newMonth / 12);
            this.currentMonth = (newMonth % 12) + 1;
            await this.loadCalendar();
        },
    }
};
</script>

<style scoped>
.calendarHeader {
    display: flex;
    gap: 1em;
    align-items: center;
}

.calendarHeader h3 {
    margin: 0;
    padding: 0;
}

.headerArrow {
    cursor: pointer;
    font-size: 200%;
}

.headerMonth {
    flex: 8em 0 0;
}

.holidayName {
    overflow: hidden;
    white-space: nowrap;
    font-size: 80%;
}

.kalender {
    display: grid;
    width: 100%;
    grid-template-columns: repeat(5, minmax(0, 1fr)) repeat(2, minmax(0, 0.5fr));
    grid-template-rows: auto repeat(6, minmax(3em, auto));
}

.headerCell {
    overflow: hidden;
    font-weight: bolder;
    border: 1px solid hsl(0, 0%, 85%);
}

.dayCell {
    border: 1px solid hsl(0, 0%, 85%);
    padding: 0.2em;
}

.dayCell:hover {
    border: 2px solid hsl(0, 0%, 10%);
}

.disabled {
    color: hsl(0, 0%, 70%);
}

.weekend {
    background-color: hsl(0, 0%, 97%);
}

.holiday {
    background-color: hsl(0, 100%, 95%);
}

.today {
    border: 2px solid hsl(0, 0%, 10%);
}

.dayHeader {
    display: flex;
    flex-wrap: wrap;
    align-items: center;
}

.dayNumber {
    font-weight: bolder;
    flex-grow: 1;
}

.dayData {
    font-size: 80%;
}</style>
