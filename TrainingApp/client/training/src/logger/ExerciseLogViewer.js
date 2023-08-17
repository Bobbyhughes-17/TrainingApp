import React, { useState, useEffect, useContext } from "react";
import DatePicker from "react-datepicker";
import "react-datepicker/dist/react-datepicker.css";
import "./Log.css";
import UserContext from "../contexts/UserContext";
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faCalendar } from "@fortawesome/free-solid-svg-icons";
import { getSetLogsByUserAndDate } from "../Managers/SetLogManager";
import { getExerciseById } from "../Managers/ExerciseManager";

export const ExerciseLogViewer = () => {
  const { user } = useContext(UserContext);
  const [selectedDate, setSelectedDate] = useState(new Date());
  const [logs, setLogs] = useState([]);

  useEffect(() => {
    // check if both user and date are available
    if (user && selectedDate instanceof Date) {
      // getch set logs by user id and date
      getSetLogsByUserAndDate(user.id, selectedDate.toISOString().split("T")[0])
        .then((data) => {
          // grouping logs by exercise id so they are together in the ui
          const groupedLogs = groupLogsByExerciseId(data);
          //grabbing the unique ids from grouped logs
          const exerciseIds = Object.keys(groupedLogs);
          // getting the exercises that match to the exercises ids
          return Promise.all(exerciseIds.map((id) => getExerciseById(id))).then(
            (exercises) => {
              // adding exercise names to the group logs
              exercises.forEach((exercise) => {
                groupedLogs[exercise.id] = {
                  logs: groupedLogs[exercise.id],
                  name: exercise.name,
                };
              });

              return groupedLogs;
            }
          );
        })
        .then((groupedLogs) => setLogs(groupedLogs));
    }
  }, [selectedDate, user]);
// grouping logs by exerciseid
  const groupLogsByExerciseId = (logs) => {
    return logs.reduce((acc, log) => {
      if (!acc[log.exerciseId]) {
        acc[log.exerciseId] = [];
      }
      acc[log.exerciseId].push(log);
      return acc;
    }, {});
  };

  return (
    <div className="log-root">
      <h2>Exercise Logs</h2>
      <div className="date-picker-container">
        <div className="date-picker-root">
        <FontAwesomeIcon icon={faCalendar} className="date-picker-icon" />
          <DatePicker
            selected={selectedDate}
            onChange={(date) => {
              if (date instanceof Date) {
                setSelectedDate(date);
              }
            }}
          />
        </div>
      </div>

      <div>
        {Object.entries(logs).map(([exerciseId, exerciseData]) => (
          <div key={exerciseId}>
            <h3>{exerciseData.name}</h3>
            {exerciseData.logs.map((log, index) => (
              <div key={log.id}>
                <h4>Set {index + 1}</h4>
                <p>Reps: {log.repetitions}</p>
                <p>Weight: {log.weight} lbs</p>
              </div>
            ))}
          </div>
        ))}
      </div>
    </div>
  );
};
